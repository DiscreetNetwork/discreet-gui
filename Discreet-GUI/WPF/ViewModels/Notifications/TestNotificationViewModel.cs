using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Notifications
{
    class TestNotificationViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> CloseCommand { get; set; }

        float _opacity = 1;
        public float Opacity { get => _opacity; set { _opacity = value; OnPropertyChanged(nameof(Opacity)); } }
        public TestNotificationViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            CloseCommand = ReactiveCommand.Create(() =>
            {
                Opacity = 0;
                Task.Delay(200).ContinueWith(_ => navigationServiceFactory.DismissNotification().Navigate());
            });
        }
    }
}
