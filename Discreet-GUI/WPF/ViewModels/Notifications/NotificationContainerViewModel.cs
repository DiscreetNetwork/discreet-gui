using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Attributes;
using WPF.Factories.Navigation;
using WPF.Factories.ViewModel;
using WPF.Services;
using WPF.Stores;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Notifications
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

        public void AddNotification(NotificationViewModelBase nvm)
        {
            nvm.NotificationDismissed += OnNotificationDismissed;
        }

        void OnNotificationDismissed(string notificationId)
        {
            
        }
    }
}
