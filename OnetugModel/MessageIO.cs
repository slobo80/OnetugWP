using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Security;
using KulAid.Helpers;
namespace OnetugModel
{
    public class MessageIO<T>
    {
        private string _url = string.Empty;
        private string _fileName = string.Empty;
        private Action<List<T>> _callback;
        private Action<Exception> _error;

        public MessageIO(string url, string filename)
        {
#if DEBUG
            _url = url.Replace(".xml", "Test.xml");
#else
            _url = url;
#endif
        }

        public void SaveMessages(List<T> messages, Action completed)
        {
            IsolatedStorage<List<T>> store = new IsolatedStorage<List<T>>();
            store.BeginSave(_fileName, messages, completed, null);
        }

        internal void LoadMessagesFromFile(Action<List<T>> success)
        {
            IsolatedStorage<List<T>> store = new IsolatedStorage<List<T>>();
            store.BeginLoad(_fileName, success, null);
        }

        public void DownloadFile(Action<List<T>> success, Action<Exception> error)
        {
            bool networkAvailable = NetworkInterface.GetIsNetworkAvailable();
            if (networkAvailable)
            {
                _callback = success;
                _error = error;
                ExecuteDownload();
            }
            else
            {
                success(null);
            }
        }

        private void ExecuteDownload()
        {
            var hRequest = HttpWebRequest.CreateHttp(_url);
            Debug.WriteLine("Downloading file from: " + _url);

            hRequest.BeginGetResponse(i =>
            {
                try
                {
                    List<T> result = null;
                    if (hRequest.HaveResponse)
                    {
                        var response = hRequest.EndGetResponse(i);
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                //string xmlData = reader.ReadToEnd();
                                IO<List<T>> serializer = new IO<List<T>>();
                                result = serializer.DeserializeStream(stream);
                            }
                        }
                        _callback(result);
                    }
                }
                catch (WebException we)
                {
                    Debug.WriteLine("Failed to download the file: " + we.Message);
                    _error(we);
                }
                catch (SecurityException se)
                {
                    string statusString = se.Message;
                    if (string.IsNullOrEmpty(statusString))
                    {
                        Debug.WriteLine(se.InnerException.Message);
                    }
                    _error(se);
                }
                catch (Exception ex)
                {
                    _error(ex);
                }
            }, null);

        }
    }
}
