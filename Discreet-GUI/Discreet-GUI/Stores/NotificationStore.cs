using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Stores
{
    public class NotificationStore
    {
        private object _lock = new object();
        private int _maxNotificationCount = 6;

        public ObservableCollection<NotificationBody> Notifications { get; set; } = new ObservableCollection<NotificationBody>();
        private ConcurrentQueue<NotificationBody> _notificationQueue = new ConcurrentQueue<NotificationBody>();

        public void Add(NotificationBody notification)
        {
            if(Notifications.Count < _maxNotificationCount)
            {
                notification.OnDismissed += OnDismissed;
                _ = notification.StartDelayedDismiss();
                Notifications.Insert(0, notification);
            }
            else
            {
                _notificationQueue.Enqueue(notification);
            }
        }

        void OnDismissed(NotificationBody notificationBody)
        {
            lock (_lock)
            {
                notificationBody.OnDismissed -= OnDismissed;
                Notifications.Remove(notificationBody);
            }

            while(Notifications.Count < _maxNotificationCount && !_notificationQueue.IsEmpty)
            {
                if(_notificationQueue.TryDequeue(out var fromQueue))
                {
                    Add(fromQueue);
                }
            }
        }
    }

    public class NotificationBody
    {
        public event Action<NotificationBody> OnDismissed;

        public string Text { get; set; }
        public string Color { get; set; }

        public NotificationBody(string text, string color)
        {
            Text = text;
            Color = color;
        }

        public async Task StartDelayedDismiss()
        {
            await Task.Delay(3000);
            DismissCommand();
        }

        void DismissCommand()
        {
            OnDismissed?.Invoke(this);
        }
    }
}
