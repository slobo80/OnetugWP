using System;
using System.Collections.Generic;
using OnetugModel;

namespace OnetugServices
{
    public interface INewsFetcher
    {
        string FileUrl { get; set; }
        string ServiceUrl { get; set; }
        void GetNewsItems(Action<List<NewsModel>> success, Action<Exception> error);
    }
}
