using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Services.Navigation.Common;
using WPF.Stores;
using WPF.ViewModels.Common;

namespace WPF.Services.Navigation
{
    public class DismissNotificationNavigationService : INavigationService
    {
        private readonly NotificationStore _notificationStore;

        public DismissNotificationNavigationService(NotificationStore notificationStore)
        {
            _notificationStore = notificationStore;
        }

        public void Navigate() => _notificationStore.CurrentNotificationViewModel = null;
    }
}
