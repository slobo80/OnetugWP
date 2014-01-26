using System;
using OnetugModel;
using System.Collections.Generic;
namespace OnetugServices
{
    public class CloudFileSponsorsFetcher : ISponsorsFetcher
    {
        public string FileUrl { get; set; }
        public string ServiceUrl { get; set; }

        string Filename = "Sponsors.xml";

        public void GetSponsors(Action<List<SponsorModel>> success, Action<Exception> error)
        {
            MessageIO<SponsorModel> items = new MessageIO<SponsorModel>(FileUrl, Filename);
            items.DownloadFile(success, error);
        }
    }
}
