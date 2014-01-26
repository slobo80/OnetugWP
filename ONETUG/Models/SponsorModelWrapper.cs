using OnetugModel;
namespace Onetug
{
    public class SponsorModelWrapper : SponsorModel
    {
        ImageWrapper _image;
        public ImageWrapper Image
        {
            get
            {
                if (((null == _image) || (null == _image.Value)) && !string.IsNullOrEmpty(ImageName))
                {
                    _image = new ImageWrapper(ImageName, ImageUrl);
                }
                return _image;
            }
            set
            {
                _image = value;
                RaisePropertyChanged("Image");
            }
        }
    }
}
