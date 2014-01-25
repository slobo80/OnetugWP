using System;
using System.Collections.ObjectModel;
namespace AboutPage
{
    public class StaticInfoModel
    {
        public ObservableCollection<History> UpdateHistory { get; set; }
    }

    public class History
    {
        public string Version { get; set; }
        public string Changes { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
