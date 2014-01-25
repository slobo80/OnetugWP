using Microsoft.Phone.Controls;
using System;
using Coding4Fun.Phone.Controls;
using KulAid.Helpers.Analytics;

namespace AboutPage
{
    public partial class About : PhoneApplicationPage
    {
        AboutViewModel _viewModel = new AboutViewModel();
        private AnalyticsTracker _tracker = new AnalyticsTracker();

        public About()
        {
            InitializeComponent();
            DataContext = _viewModel;
            _tracker.Track(EventCategories.Feature, EventAction.About);
        }

        private void Tile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selection = sender as Tile;
            string command = selection.Tag.ToString();
            switch (command)
            {
                case "Twitter":
                    {
                        _tracker.Track(EventCategories.Feature, EventAction.AboutTwitter);
                        _viewModel.TwitterCommand.Execute(null);
                        break;
                    }
                case "Facebook":
                    {
                        _tracker.Track(EventCategories.Feature, EventAction.AboutFacebook);
                        _viewModel.FacebookCommand.Execute(null);
                        break;
                    }
                case "Website":
                    {
                        _tracker.Track(EventCategories.Feature, EventAction.AboutWebsite);
                        _viewModel.ViewWebsiteCommand.Execute(null);
                        break;
                    }
                case "Review":
                    {
                        _tracker.Track(EventCategories.Feature, EventAction.AboutReview);
                        _viewModel.ReviewCommand.Execute(null);
                        break;
                    }
                case "Feedback":
                    {
                        _tracker.Track(EventCategories.Feature, EventAction.AboutFeedback);
                        _viewModel.SupportQuestionCommand.Execute(null);
                        break;
                    }
                default:
                    break;
            }
        }

        private void hubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _viewModel.MarketplaceSearchCommand.Execute(null);
        }
    }
}