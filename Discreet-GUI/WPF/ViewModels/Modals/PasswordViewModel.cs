using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;

namespace WPF.ViewModels.Modals
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class PasswordViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> EnterCommand { get; set; }

        public PasswordViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            EnterCommand = ReactiveCommand.Create(navigationServiceFactory.CreateModalNavigationService().Navigate);
        }
    }
}
