using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Threading;
using System.Xml.Linq;
using KulAid.Helpers;
using Microsoft.Phone.Net.NetworkInformation;
using OnetugModel;
using OnetugServices;

namespace Onetug
{
    public class MainViewModel : ViewModelBase
    {
        private int _activeCallCount = 0;
        private INewsFetcher _newsService;
        private ISponsorsFetcher _sponsorService;
        private IEventsFetcher _eventService;

        public List<NewsModel> NewsLinks { get; private set; }

        public List<SponsorModelWrapper> Sponsors { get; private set; }

        public List<EventModel> Events { get; private set; }

        public Settings Settings { get; set; }

        LoadingState _loadingState;
        public LoadingState LoadingState
        {
            get
            {
                return _loadingState;
            }
            set
            {

                _loadingState = value;
                RaisePropertyChanged("LoadingState");
            }
        }

        LoadingState _loadingStateEvents;
        public LoadingState LoadingStateEvents
        {
            get
            {
                return _loadingStateEvents;
            }
            set
            {

                _loadingStateEvents = value;
                RaisePropertyChanged("LoadingStateEvents");
            }
        }

        public int ActiveCallCount
        {
            get
            {
                return _activeCallCount;
            }
            set
            {

                _activeCallCount= value;
                RaisePropertyChanged("ActiveCallCount");
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            ActiveCallCount = 0;
            LoadSettings();
            LoadNews();
            LoadSponsors();
            LoadEvents();
            IsDataLoaded = true;
        }

        public MainViewModel(INewsFetcher newsService, ISponsorsFetcher sponsorService, IEventsFetcher eventService)
        {
            _newsService = newsService;
            _sponsorService = sponsorService;
            _eventService = eventService;
        }

        private void LoadSettings()
        {
            StreamResourceInfo xml = Application.GetResourceStream(new Uri("/Onetug;component/Data/Settings.xml", UriKind.Relative));
            XDocument settingsDoc = XDocument.Load(xml.Stream);

            Settings = new Settings()
            {
                NewsUrl = settingsDoc.Root.Element("newsUrl").Value,
                SponsorsUrl = settingsDoc.Root.Element("sponsorsUrl").Value,
                EventsUrl = settingsDoc.Root.Element("eventsUrl").Value,
            };
        }

        private void LoadNews()
        {
            SmartDispatcher.Initialize();
            LoadingState = LoadingState.LOADING;

            if (NetworkInterface.GetIsNetworkAvailable() && !string.IsNullOrEmpty(App.ViewModel.Settings.NewsUrl))
            {
                ActiveCallCount++;
                _newsService.FileUrl = App.ViewModel.Settings.NewsUrl;
                _newsService.GetNewsItems((items) =>
                    {
                        SmartDispatcher.BeginInvoke(() =>
                        {
                            NewsLinks = items;
                            LoadingState = LoadingState.COMPLETED;
                            RaisePropertyChanged("NewsLinks");
                            ActiveCallCount--;
                        });
                    }, (ex) =>
                    {
                        SmartDispatcher.BeginInvoke(() =>
                        {
                            LoadingState = LoadingState.ERROR;
                            ActiveCallCount--;
                        });
                    });
            }
            else
            {
                LoadingState = LoadingState.NONETWORK;
            }
        }

        private void LoadSponsors()
        {
            SmartDispatcher.Initialize();
            //LoadingStateEvents = LoadingState.LOADING;

            if (NetworkInterface.GetIsNetworkAvailable() && !string.IsNullOrEmpty(App.ViewModel.Settings.SponsorsUrl))
            {
                ActiveCallCount++;
                _sponsorService.FileUrl = App.ViewModel.Settings.SponsorsUrl;
                _sponsorService.GetSponsors((items) =>
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        SponosorModelAdapter adapter = new SponosorModelAdapter(items);
                        Sponsors = adapter.Sponsors();
                        //LoadingStateEvents = Onetug.LoadingState.COMPLETED;
                        //RaisePropertyChanged("LoadingState");
                        RaisePropertyChanged("Sponsors");
                        ActiveCallCount--;
                    });
                }, (ex) =>
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        //LoadingState = LoadingState.ERROR;
                        //RaisePropertyChanged("LoadingState");
                        ActiveCallCount--;
                    });
                });
            }
        }

        private void LoadEvents()
        {
            LoadingStateEvents = LoadingState.COMPLETED;
            if (NetworkInterface.GetIsNetworkAvailable() && !string.IsNullOrEmpty(App.ViewModel.Settings.EventsUrl))
            {
                ActiveCallCount++;
                _eventService.ServiceUrl = App.ViewModel.Settings.EventsUrl;
                _eventService.GetEvents((items) =>
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        Events = items;
                        LoadingStateEvents = LoadingState.COMPLETED;
                        RaisePropertyChanged("Events");
                        ActiveCallCount--;
                    });
                }, (ex) =>
                {
                    SmartDispatcher.BeginInvoke(() =>
                    {
                        LoadingStateEvents = LoadingState.ERROR;
                        ActiveCallCount--;
                    });
                });
            }
            else
            {
                LoadingStateEvents = LoadingState.NONETWORK;
            }
        }
    }
}