using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Input;
namespace KulAid.Helpers.AboutPage
{
    [DataContract()]
    public abstract class VMBase : ObservableObject
    {
        [IgnoreDataMemberAttribute()]
        public RelayCommand RefreshUICommand { get; private set; }

        public VMBase()
        {
            RegisterCommands();
        }

        protected virtual void RegisterCommands() 
        {
            RefreshUICommand = new RelayCommand(RefreshUI);
        }

        protected virtual void RefreshUI() { }
    }
}
