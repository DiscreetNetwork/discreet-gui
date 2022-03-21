using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Start;

namespace WPF.ViewModels.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class WalletDetailsViewModel : ViewModelBase
    {
        public ObservableCollection<string> NetworkTypes { get; set; } = new ObservableCollection<string> { "Mainnet", "Testnet" };
        public int SelectedNetworkTypeIndex { get; set; }

        ReactiveCommand<Unit, Unit> NavigateWalletCreatedViewCommand { get; set; }

        public WalletDetailsViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateWalletCreatedViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletCreatedSuccessfullyViewModel>().Navigate);
        }
    }
}
