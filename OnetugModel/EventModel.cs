using System.Runtime.Serialization;
using System;
namespace OnetugModel
{
    [DataContract()]
    public class EventModel
    {
        [DataMember()]
        public string Title { get; set; }

        [DataMember()]
        public string Description { get; set; }

        [DataMember()]
        public string Author { get; set; }

        [DataMember()]
        public string Comments { get; set; }

        [DataMember()]
        public DateTime PubDate { get; set; }
    }
}
