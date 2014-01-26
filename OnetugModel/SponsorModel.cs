using System.Runtime.Serialization;
using System;
using KulAid.Helpers;

namespace OnetugModel
{
    [DataContract()]
    public class SponsorModel : VMBase
    {
        [DataMember()]
        public bool Active { get; set; }
        [DataMember()]
        public string Name { get; set; }
        [DataMember()]
        public string Website { get; set; }
        [DataMember()]
        public string ImageUrl { get; set; }
        [DataMember()]
        public string ImageName { get; set; }

        //ImageWrapper _image;
        //[IgnoreDataMember]
        //public ImageWrapper Image
        //{
        //    get
        //    {
        //        if (((null == _image) || (null == _image.Value)) && !string.IsNullOrEmpty(Name))
        //        {
        //            _image = new ImageWrapper(Name, ImageUrl);
        //        }
        //        return _image;
        //    }
        //    set
        //    {
        //        _image = value;
        //        RaisePropertyChanged("Image");
        //    }
        //}
    }
}
