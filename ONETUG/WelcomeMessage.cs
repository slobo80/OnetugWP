using KulAid.Helpers;
using System.Windows;
using Coding4Fun.Phone.Controls;
using System.Windows.Media;
using System.Windows.Controls;
using KulAid.Helpers.Analytics;
using Coding4Fun.Phone.Controls.Data;
namespace Onetug
{
    public class WelcomeMessage
    {
        private static string Version = PhoneHelper.GetAppAttribute("Version");
        public static void DisplayWelcomeMessage()
        {
            if (WelcomeMessage.IsFirstRun())
            {
                string title = "Welcome to ONETUG";
                string message = "Thanks for choosing ONETUG app. Feel free to send your suggestions directly from the About page.\n\n Enjoy!";

                MessagePrompt msg = new MessagePrompt();
                msg.Title = title;
                msg.Body = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap };
                msg.IsCancelVisible = false;
                msg.Completed += new System.EventHandler<PopUpEventArgs<string, PopUpResult>>(msg_Completed);
                msg.Show();

                App.Tracker.Track(EventCategories.Usage, EventAction.NewUser);
            }
            else if (WelcomeMessage.DisplayNewUINotes())
            {
                string title = "";
                string message = "";

                MessagePrompt msg = new MessagePrompt();
                msg.Title = title;
                msg.Body = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap };
                msg.IsCancelVisible = false;
                msg.Completed += new System.EventHandler<PopUpEventArgs<string, PopUpResult>>(msg_Completed);
                msg.Show();

                App.Tracker.Track(EventCategories.Usage, EventAction.UpgradeUser);
            }
            else
            {
                App.Tracker.Track(EventCategories.Usage, EventAction.ExistingUser);
            }
        }

        static void msg_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            SetRunCount(2);
        }

        private static void SetRunCount(int count)
        {
            AppSettings appSettings = new AppSettings();
            appSettings.AddOrUpdateValue("RunCount", count);
            ReadUpdateNote();
        }

        private static bool IsFirstRun()
        {
            bool result = true;
            AppSettings appSettings = new AppSettings();
            int runCount = appSettings.GetValueOrDefault("RunCount", 0);
            if (0 < runCount)
            {
                result = false;
            }

            return result;
        }

        private static bool DisplayNewUINotes()
        {
            bool result = false;
            AppSettings appSettings = new AppSettings();
            int runCount = appSettings.GetValueOrDefault("RunCount", 0);
            bool hasReadUpdateNote = appSettings.GetValueOrDefault(Version, false);
            if (0 < runCount && !hasReadUpdateNote)
            {
                ReadUpdateNote();
                result = true;
            }

            return result;
        }

        private static void ReadUpdateNote()
        {
            AppSettings appSettings = new AppSettings();
            appSettings.AddOrUpdateValue(Version, true);
        }

    }
}
