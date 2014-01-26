using System.ServiceModel.Syndication;
using System;
using OnetugModel;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.IO;
using System.Linq;
namespace OnetugServices
{
    public class FeedEventsFetcher : IEventsFetcher
    {
        public string ServiceUrl { get; set; }
        public void GetEvents(Action<List<EventModel>> success, Action<Exception> error)
        {
            List<EventModel> result = new List<EventModel>();
            var c = new WebClient();
            c.DownloadStringCompleted += (o, e) =>
            {
                try
                {
                    if (e.Error == null)
                    {
                        result = ParseItemFeed(e.Result);
                        success(result);
                    }
                }
                catch (Exception ex)
                {
                    error(ex);
                }
            };

            c.DownloadStringAsync(new Uri(ServiceUrl));
        }

        private List<EventModel> ParseItemFeed(string xml)
        {
            List<EventModel> result = new List<EventModel>();

            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                var feed = SyndicationFeed.Load(reader);
                foreach (var item in feed.Items)
                {
                    var eventItem = new EventModel
                    {
                        Description = item.Summary.Text,
                        Author = item.Authors.Single().Email,
                        PubDate=item.PublishDate.Date,
                        //Comments=item.l
                        Title=item.Title.Text
                    };
                    result.Add(eventItem);
                }
            }
            return result;
        }
    }
}
