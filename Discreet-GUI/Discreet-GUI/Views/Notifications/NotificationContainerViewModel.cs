using System.Collections.ObjectModel;
using Discreet_GUI.Attributes;
using Discreet_GUI.Stores;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Views.Notifications
{
    [AssemblyScanIgnore("Registered manually as a singleton")]
    public class NotificationContainerViewModel : ViewModelBase
    {
        private readonly NotificationStore _notificationStore;

        public ObservableCollection<NotificationBody> Notifications => _notificationStore.Notifications;

        public NotificationContainerViewModel() { }
        public NotificationContainerViewModel(NotificationStore notificationStore)
        {
            _notificationStore = notificationStore;
        }
    }
}
