using System.ComponentModel;
using System.Runtime.Serialization;

namespace KulAid.Helpers.AboutPage
{
    [DataContract()]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public ObservableObject()
        {
            
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
            {
                RaisePropertyChanged(name);
            }
        }
    }
}