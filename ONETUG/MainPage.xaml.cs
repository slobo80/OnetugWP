using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using OnetugServices;
using KulAid.Helpers.Analytics;
using KulAid.Helpers.ErrorReporting;
using Onetug.Localization;
using System.Reflection;

namespace Onetug
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool _executePageLoadActions = true;

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_executePageLoadActions)
            {
                _executePageLoadActions = false;
                WelcomeMessage.DisplayWelcomeMessage();
                AskforReview();
                ErrorReporter.SendErrorLog(Constants.EmailAddress, Constants.AppName);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            DataContext = App.ViewModel;
        }

        private void SponsorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            var sponsor = e.AddedItems[0] as SponsorModelWrapper;

            if (!String.IsNullOrEmpty(sponsor.Website))
            {
                WebBrowserTask web = new WebBrowserTask();
                string url = sponsor.Website.Replace("http://", string.Empty);
                url = "http://" + url;
                web.Uri = new Uri(url, UriKind.Absolute);
                web.Show();
            }

            ((ListBox)sender).SelectedItem = null;
        }

        private void ApplicationBarAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage;component/About.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            App.ViewModel.LoadData();
        }

        private void AskforReview()
        {
            if (ReviewBugger.IsTimeForReview())
            {
                ReviewBugger.PromptUser(
                    () => { App.Tracker.Track(EventCategories.Review, EventAction.ReviewYes); },
                    () => { App.Tracker.Track(EventCategories.Review, EventAction.ReviewLater); },
                    () => { App.Tracker.Track(EventCategories.Review, EventAction.ReviewNo); });
            }
        }

        private void eventsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri(AppResources.Website, UriKind.Absolute);
            browser.Show();
        }
    }
}