using System;
using System.Collections.Generic;
using OnetugModel;

namespace OnetugServices
{
    public interface ISponsorsFetcher
    {
        string FileUrl { get; set; }
        string ServiceUrl { get; set; }
        void GetSponsors(Action<List<SponsorModel>> success, Action<Exception> error);
    }
}
