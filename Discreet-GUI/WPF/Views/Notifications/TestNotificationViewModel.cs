using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF.Attributes;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;

namespace WPF.Views.Notifications
{
    [AssemblyScanIgnore("This notification will get instantiated manually by the NotificationService")]
    class TestNotificationViewModel : NotificationViewModelBase
    {
        float _opacity = 1;
        public float Opacity { get => _opacity; set { _opacity = value; OnPropertyChanged(nameof(Opacity)); } }


        public TestNotificationViewModel(string text) : base(text)
        {
            //_ = AutoDismiss();
        }

        public async Task Dismiss()
        {
            Opacity = 0;
            await Task.Delay(200);
            DismissNotification();
        }

        public async Task AutoDismiss()
        {
            await Task.Delay(5000);
            Opacity = 0;
            await Task.Delay(200);
            DismissNotification();
        }
    }
}
