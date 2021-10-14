using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Account;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Layouts.Account;

namespace WPF.ViewModels.Start
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class WalletCreatedSuccessfullyViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateNextCommand { get; set; }
        public WalletCreatedSuccessfullyViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateNextCommand = ReactiveCommand.Create(() => 
            {
                navigationServiceFactory.CreateAccountNavigation<AccountLeftNavigationLayoutViewModel>().Navigate();
                navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
            });
        }
    }
}
