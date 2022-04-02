using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Factories.Navigation;
using WPF.Stores;
using WPF.ViewModels.Common;
using WPF.ViewModels.Notifications;

namespace WPF.Services
{
    public class NotificationService
    {
        private readonly NotificationStore _notificationStore;

        public NotificationService(NotificationStore notificationStore)
        {
            _notificationStore = notificationStore;
        }

        public void Display(string content)
        {
            _notificationStore.Add(new NotificationBody(content));
        }
    }
}
