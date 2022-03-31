using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.Stores
{
    public class NotificationStore
    {
        public event Action CurrentNotificationViewModelChanged;
        private ViewModelBase _currentNotificationViewModel;
        public ViewModelBase CurrentNotificationViewModel { get => _currentNotificationViewModel; set { _currentNotificationViewModel = value; CurrentNotificationViewModelChanged?.Invoke(); } }
    }
}
