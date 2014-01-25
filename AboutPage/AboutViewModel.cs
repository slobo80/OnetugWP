using System.Reflection;
using KulAid.Helpers.AboutPage;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Tasks;
using System;
using Microsoft.Phone.Marketplace;
using System.Windows.Resources;
using System.Xml.Linq;
using Coding4Fun.Phone.Controls.Data;

namespace AboutPage
{
    public class AboutViewModel : VMBase
    {
        public const string MarketplaceSearchTerm = "Slobodan Stipic";

        public AboutViewModel()
        {
            LoadValuesFromResource<AppResources>();
            LoadStaticInfo();
        }

        public void LoadValuesFromResource<T>()
        {
            var targetType = GetType();
            var sourceType = typeof(T);
            foreach (var targetProperty in targetType.GetProperties())
            {
                var sourceProperty = sourceType.GetProperty(targetProperty.Name, BindingFlags.Static | BindingFlags.Public);
                if (sourceProperty != null)
                {
                    targetProperty.SetValue(this, sourceProperty.GetValue(null, null), null);
                }
            }
        }

        private StaticInfoModel _history;
        public StaticInfoModel History
        {
            get { return _history; }
            set
            {
                _history = value;
            }
        }

        private string _appTitle;
        /// <Summary>A string value for the AppTitle</Summary>
        public string AppTitle
        {
            get { return _appTitle; }
            set
            {
                _appTitle = value;
                RaisePropertyChanged("AppTitle");
            }
        }

        private string _about;
        /// <Summary>A string value for the About</Summary>
        public string About
        {
            get { return _about; }
            set
            {
                _about = value;
                RaisePropertyChanged("About");
            }
        }

        private string _buy;
        /// <Summary>A string value for the Buy</Summary>
        public string Buy
        {
            get { return _buy; }
            set
            {
                _buy = value;
                RaisePropertyChanged("Buy");
            }
        }

        private string _buyTheApp;
        /// <Summary>A string value for the BuyTheApp</Summary>
        public string BuyTheApp
        {
            get { return _buyTheApp; }
            set
            {
                _buyTheApp = value;
                RaisePropertyChanged("BuyTheApp");
            }
        }

        private string _companyUrl;
        /// <Summary>A string value for the CompanyUrl</Summary>
        public string CompanyUrl
        {
            get { return _companyUrl; }
            set
            {
                _companyUrl = value;
                RaisePropertyChanged("CompanyUrl");
            }
        }
        private string _otherApps;
        /// <Summary>A string value for the Other apps</Summary>
        public string OtherApps
        {
            get { return _otherApps; }
            set
            {
                _otherApps = value;
                RaisePropertyChanged("OtherApps");
            }
        }

        private string _copyright;
        /// <Summary>A string value for the Copyright</Summary>
        public string Copyright
        {
            get { return _copyright; }
            set
            {
                _copyright = value;
                RaisePropertyChanged("Copyright");
            }
        }

        private string _review;
        /// <Summary>A string value for the Review</Summary>
        public string Review
        {
            get { return _review; }
            set
            {
                _review = value;
                RaisePropertyChanged("Review");
            }
        }

        private string _reviewTheApp;
        /// <Summary>A string value for the ReviewTheApp</Summary>
        public string ReviewTheApp
        {
            get { return _reviewTheApp; }
            set
            {
                _reviewTheApp = value;
                RaisePropertyChanged("ReviewTheApp");
            }
        }

        private string _support;
        /// <Summary>A string value for the Support</Summary>
        public string Support
        {
            get { return _support; }
            set
            {
                _support = value;
                RaisePropertyChanged("Support");
            }
        }

        private string _supportMessage;
        /// <Summary>A string value for the SupportMessage</Summary>
        public string SupportMessage
        {
            get { return _supportMessage; }
            set
            {
                _supportMessage = value;
                RaisePropertyChanged("SupportMessage");
            }
        }

        private string _supportEmail;
        /// <Summary>A string value for the SupportEmail</Summary>
        public string SupportEmail
        {
            get { return _supportEmail; }
            set
            {
                _supportEmail = value;
                RaisePropertyChanged("SupportEmail");
            }
        }

        private string _facebookText;
        /// <Summary>A string value for the Facebook text</Summary>
        public string FacebookText
        {
            get { return _facebookText; }
            set
            {
                _facebookText = value;
                RaisePropertyChanged("FacebookText");
            }
        }

        private string _facebookLink;
        /// <Summary>A string value for the Facebook text</Summary>
        public string FacebookLink
        {
            get { return _facebookLink; }
            set
            {
                _facebookLink = value;
                RaisePropertyChanged("FacebookLink");
            }
        }

        private string _twitterText;
        /// <Summary>A string value for the Facebook text</Summary>
        public string TwitterText
        {
            get { return _twitterText; }
            set
            {
                _twitterText = value;
                RaisePropertyChanged("TwitterText");
            }
        }

        private string _twitterLink;
        /// <Summary>A string value for the Facebook text</Summary>
        public string TwitterLink
        {
            get { return _twitterLink; }
            set
            {
                _twitterLink = value;
                RaisePropertyChanged("TwitterLink");
            }
        }

        private bool? _trialMode;
        public bool IsTrialMode
        {
            get
            {
                if (_trialMode == null)
                {
                    var s = new LicenseInformation();
#if DEBUG
                    _trialMode = true;
#else
                    _trialMode = new bool?(s.IsTrial());
#endif
                }
                return _trialMode.Value;
            }
        }

        public Visibility BuyPanelVisible
        {
            get { return IsTrialMode || DesignerProperties.IsInDesignTool ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string ApplicationVersion
        {
            get
            {
                if (DesignerProperties.IsInDesignTool)
                    return "version x.x";

                return PhoneHelper.GetAppAttribute("Version").Replace(".0.0", "");
            }
        }

        public ICommand SupportQuestionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var emailComposeTask = new EmailComposeTask
                    {
                        To = SupportEmail,
                        Subject =
                            Support + " " + AppTitle + " " +
                            ApplicationVersion
                    };
                    emailComposeTask.Show();
                });
            }
        }

        public ICommand BuyCommand
        {
            get
            {
                return new RelayCommand(() => new MarketplaceDetailTask().Show());
            }
        }

        public ICommand ReviewCommand
        {
            get
            {
                return new RelayCommand(() => new MarketplaceReviewTask().Show());
            }
        }

        public ICommand ViewWebsiteCommand
        {
            get
            {
                return new RelayCommand(() => new WebBrowserTask { Uri = new Uri(CompanyUrl, UriKind.Absolute) }.Show());
            }
        }
        public ICommand MarketplaceSearchCommand
        {
            get
            {
                return new RelayCommand(() => new MarketplaceSearchTask { SearchTerms = MarketplaceSearchTerm }.Show());
            }
        }

        public ICommand FacebookCommand
        {
            get
            {
                return new RelayCommand(() => new WebBrowserTask { Uri = new Uri(FacebookLink, UriKind.Absolute) }.Show());
            }
        }
        public ICommand TwitterCommand
        {
            get
            {
                return new RelayCommand(() => new WebBrowserTask { Uri = new Uri(TwitterLink, UriKind.Absolute) }.Show());
            }
        }

        private void LoadStaticInfo()
        {
            StaticInfoManager staticInfo = new StaticInfoManager();
            staticInfo.RetrieveSettingsFromAssembly(
                (p) =>
                {
                    History = p;
                },
                (p) =>
                {

                });
        }

    }
}