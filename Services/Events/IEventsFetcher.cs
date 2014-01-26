using System;
using System.Collections.Generic;
using OnetugModel;
namespace OnetugServices
{
    public interface IEventsFetcher
    {
        string ServiceUrl { get; set; }
        void GetEvents(Action<List<EventModel>> success, Action<Exception> error);
    }
}
