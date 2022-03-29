using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Start;

namespace WPF.ViewModels.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class WalletDetailsViewModel : ViewModelBase
    {
        private readonly NewWalletCache _newWalletCache;

        public ObservableCollection<string> NetworkTypes { get; set; } = new ObservableCollection<string> { "Mainnet", "Testnet" };
        public int SelectedNetworkTypeIndex { get; set; }

        ReactiveCommand<Unit, Unit> NavigateWalletCreatedViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public string WalletName { get => _newWalletCache.WalletName; set { _newWalletCache.WalletName = value; ValidateCanContinue(); } }
        public string WalletLocation { get => _newWalletCache.WalletLocation; set { _newWalletCache.WalletLocation = value; ValidateCanContinue(); } }
        public bool Bootstrap { get => _newWalletCache.Bootstrap; set => _newWalletCache.Bootstrap = value; }


        private bool _canContinue = false;
        public bool CanContinue { get => _canContinue; set { _canContinue = value; OnPropertyChanged(nameof(CanContinue)); } }


        public WalletDetailsViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache)
        {
            NavigateWalletCreatedViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletCreatedSuccessfullyViewModel>().Navigate);
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletPasswordViewModel>().Navigate);
            _newWalletCache = newWalletCache;

            ValidateCanContinue();
        }


        public void ValidateCanContinue()
        {
            if(string.IsNullOrWhiteSpace(WalletName))
            {
                CanContinue = false;
                return;
            }

            if(string.IsNullOrWhiteSpace(WalletLocation))
            {
                CanContinue = false;
                return;
            }

            CanContinue = true;
        }
    }
}
