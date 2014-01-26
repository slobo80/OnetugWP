using System.Collections.Generic;
using OnetugModel;
using System;
namespace OnetugServices
{
    public class CloudFileNewsFetcher : INewsFetcher
    {
        public string FileUrl { get; set; }
        public string ServiceUrl { get; set; }

        string Filename = "News.xml";

        public void GetNewsItems(Action<List<NewsModel>> success, Action<Exception> error)
        {
            MessageIO<NewsModel> items = new MessageIO<NewsModel>(FileUrl, Filename);
            items.DownloadFile(success,error);
        }
    }
}
