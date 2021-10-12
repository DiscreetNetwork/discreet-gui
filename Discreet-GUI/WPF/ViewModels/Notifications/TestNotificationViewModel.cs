using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Notifications
{
    class TestNotificationViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> CloseCommand { get; set; }
        public TestNotificationViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            CloseCommand = ReactiveCommand.Create(() => navigationServiceFactory.CreateModalNavigationService().Navigate());
        }
    }
}
