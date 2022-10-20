using ReactiveUI;
using Services.Daemon;
using Services.Daemon.Responses;
using Services.Daemon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Account;
using Discreet_GUI.Views.Layouts;
using Discreet_GUI.Views.Layouts.Account;

namespace Discreet_GUI.Views.CreateWallet
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
