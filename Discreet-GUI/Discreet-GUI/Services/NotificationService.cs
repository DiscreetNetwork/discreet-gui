using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Stores;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Services
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
