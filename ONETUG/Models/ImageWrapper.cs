using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization;
using System.Security;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using KulAid.Helpers;
using KulAid.Helpers.ErrorReporting;
namespace Onetug
{
    [DataContract()]
    public class ImageWrapper : VMBase
    {
        /// <summary>
        /// Lock object for thread pool save operations
        /// </summary>
        private static object saveLock = new object();

        [DataMember()]
        private string URI { get; set; }

        [DataMember()]
        private string ImageId { get; set; }

        public ImageWrapper() { }
        public ImageWrapper(string imageId, string imageUri)
        {
            URI = imageUri;
            ImageId = imageId;
            lock (saveLock)
            {
                LoadImage();
            }
        }
        [DataMember()]
        BitmapImage _value;
        /// <summary>
        /// indicates the loaded ImageSource for the image that is backed in isolated storage.
        /// </summary>
        [IgnoreDataMember()]
        public BitmapImage Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        private void LoadImage()
        {
            if (null == Value)
            {
                Value = GetImgFromIsoStore();
            }
            if (null == Value)
            {
                var request = HttpWebRequest.CreateHttp(URI);
                request.AllowReadStreamBuffering = true; // Don't want to block this thread or the UI thread on network access
                request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
            }
        }
        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = asynchronousResult.AsyncState as HttpWebRequest;
            WebResponse response = null;
            try
            {
                response = request.EndGetResponse(asynchronousResult);
            }
            catch (WebException we)
            {
                Debug.WriteLine(we.Status);
            }
            catch (SecurityException se)
            {
                string statusString = se.Message;
                if (string.IsNullOrEmpty(statusString))
                {
                    statusString = se.InnerException.Message;
                }
                ErrorReporter.HandleError(se);
                Debug.WriteLine(statusString);
            }
            // Write the stream to Bitmap as well as you can persisit on to the IsolatedStorage 
            if (response != null)
            {
                SetSource(response.GetResponseStream());
            }
        }
        private void SetSource(System.IO.Stream stream)
        {
            if (stream.Length != 0)
            {

                SmartDispatcher.BeginInvoke(() =>
                {
                    if (Value == null)
                    {
                        Value = new BitmapImage();
                    }
                    try
                    {
                        Value.SetSource(stream);
                        SaveImageToIsoStore(stream);
                    }
                    catch (IsolatedStorageException ex)
                    {
                        Debug.WriteLine("Exceptio: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ErrorReporter.HandleError(ex);
                        Debug.WriteLine("Stream is not a supported Image");
                        Debug.WriteLine("Exceptio: " + ex.Message);
                    }
                });
            }
        }
        private BitmapImage GetImgFromIsoStore()
        {
            BitmapImage result = null;
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(ImageId))
                {
                    using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(ImageId, FileMode.Open, FileAccess.Read))
                    {
                        result = new BitmapImage();
                        result.SetSource(fileStream);
                        fileStream.Close();
                    }
                }
            }
            return result;
        }
        private void SaveImageToIsoStore(Stream fileStream)
        {
            String tempImg = ImageId;

            // Create virtual store and file stream. Check for duplicate files.
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(tempImg))
                {
                    myIsolatedStorage.DeleteFile(tempImg);
                }

                IsolatedStorageFileStream isoStream = myIsolatedStorage.CreateFile(tempImg);
                //Save the image file stream rather than BitmapImage to Isolated Storage.
                byte[] content = new byte[fileStream.Length];
                fileStream.Position = 0;
                fileStream.Read(content, 0, (int)fileStream.Length);
                isoStream.Write(content, 0, (int)fileStream.Length);
                fileStream.Close();
                isoStream.Close();
            }
            GetImgFromIsoStore();
        }
    }
}
