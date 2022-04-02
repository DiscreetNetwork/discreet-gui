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
        public WalletCreatedSuccessfullyViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache, WalletCache walletCache, WalletService walletService)
        {
            var response = walletService.CreateWalletSync(newWalletCache.WalletName, newWalletCache.SeedPhrase.Select(x => x.Value).Aggregate((x, y) => x + " " + y), newWalletCache.Password);

            walletCache.Label = response.Label;
            //walletCache.TotalBalance = response.Addresses.Select(x => x.Balance).Aggregate((x, y) => x + y);
            //walletCache.Accounts = new ExtensionMethods.ObservableCollectionEx<WalletCache.WalletAddress>(response.Addresses.Select(x => new WalletCache.WalletAddress { Address = x.Address, Balance = x.Balance, Name = x.Name }));

            newWalletCache.Clear();

            NavigateNextCommand = ReactiveCommand.Create(() => 
            {
                navigationServiceFactory.CreateAccountNavigation<AccountLeftNavigationLayoutViewModel>().Navigate();
                navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
            });
        }
    }
}
