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
    public class DisplayNotificationNavigationService : INavigationService
    {
        private readonly NotificationStore _notificationStore;
        private readonly Func<ViewModelBase> _createViewModel;

        public DisplayNotificationNavigationService(NotificationStore notificationStore, Func<ViewModelBase> createViewModel)
        {
            _notificationStore = notificationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate() => _notificationStore.CurrentNotificationViewModel = _createViewModel();
    }
}
