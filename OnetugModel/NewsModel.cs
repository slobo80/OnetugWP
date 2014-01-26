using System.Runtime.Serialization;
using System;

namespace OnetugModel
{
    [DataContract()]
    public class NewsModel
    {
        [DataMember()]
        public DateTime EffectiveDate { get; set; }

        [DataMember()]
        public DateTime ExpireDate { get; set; }

        [DataMember()]
        public string Description { get; set; }
    }
}
