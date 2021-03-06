using ReactiveUI;
using Services.Daemon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Services.Caches;
using WPF.Factories.Navigation;
using WPF.Services;
using WPF.ViewModels.Common;
using WPF.Views.Layouts;
using WPF.Views.Start;

namespace WPF.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class WalletDetailsViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly NewWalletCache _newWalletCache;
        private readonly WalletCache _walletCache;
        private readonly WalletService _walletService;
        private readonly NotificationService _notificationService;

        public ObservableCollection<string> NetworkTypes { get; set; } = new ObservableCollection<string> { "Mainnet", "Testnet" };
        public int SelectedNetworkTypeIndex { get; set; }

        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public string WalletName { get => _newWalletCache.WalletName; set { _newWalletCache.WalletName = value; ValidateCanContinue(); } }
        public string WalletLocation { get => _newWalletCache.WalletLocation; set { _newWalletCache.WalletLocation = value; ValidateCanContinue(); } }
        public bool Bootstrap { get => _newWalletCache.Bootstrap; set => _newWalletCache.Bootstrap = value; }


        private bool _canContinue = false;
        public bool CanContinue { get => _canContinue; set { _canContinue = value; OnPropertyChanged(nameof(CanContinue)); } }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public WalletDetailsViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache, WalletCache walletCache, WalletService walletService, NotificationService notificationService)
        {
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletPasswordViewModel>().Navigate);
            _navigationServiceFactory = navigationServiceFactory;
            _newWalletCache = newWalletCache;
            _walletCache = walletCache;
            _walletService = walletService;
            _notificationService = notificationService;
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


        async Task CreateWalletAndContinue()
        {
            IsLoading = true;

            if (await _walletService.CreateWallet(_newWalletCache.WalletName, _newWalletCache.Mnemonic.Select(x => x).Aggregate((x, y) => x + " " + y), _newWalletCache.Password) == null)
            {
                _notificationService.Display("Failed to create wallet");
                return;
            }

            _walletCache.Label = _newWalletCache.WalletName;
            _newWalletCache.Clear();

            _navigationServiceFactory.Create<WalletCreatedSuccessfullyViewModel>().Navigate();
        }
    }
}
