using ReactiveUI;
using Services.Daemon;
using Services.Daemon.Responses;
using Services.Daemon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.Views.Account;
using WPF.Views.Layouts;
using WPF.Views.Layouts.Account;

namespace WPF.Views.Start
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class WalletCreatedSuccessfullyViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateNextCommand { get; set; }
        public WalletCreatedSuccessfullyViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache, WalletCache walletCache, WalletService walletService)
        {
            NavigateNextCommand = ReactiveCommand.Create(() => 
            {
                navigationServiceFactory.CreateAccountNavigation<AccountLeftNavigationLayoutViewModel>().Navigate();
                navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
            });
        }
    }
}
