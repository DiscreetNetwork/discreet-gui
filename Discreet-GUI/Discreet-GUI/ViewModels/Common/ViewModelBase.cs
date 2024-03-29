using ReactiveUI;
using System.ComponentModel;

namespace Discreet_GUI.ViewModels.Common
{
    public class ViewModelBase : ReactiveObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
