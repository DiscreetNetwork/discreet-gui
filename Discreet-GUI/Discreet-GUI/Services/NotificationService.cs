using Discreet_GUI.Stores;

namespace Discreet_GUI.Services
{
    public class NotificationService
    {
        private readonly NotificationStore _notificationStore;

        public NotificationService(NotificationStore notificationStore)
        {
            _notificationStore = notificationStore;
        }

        public void DisplayInformation(string content)
        {
            _notificationStore.Add(new NotificationBody(text: content, color: "#007BC2"));
        }

        public void DisplaySuccess(string content)
        {
            _notificationStore.Add(new NotificationBody(text: content, color: "#21A67A"));
        }

        public void DisplayError(string content)
        {
            _notificationStore.Add(new NotificationBody(text: content, color: "#f02e2e"));
        }
    }
}
