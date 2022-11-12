using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Services;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;
using Services.Daemon.Wallet;

namespace Discreet_GUI.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class WalletDetailsViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly NewWalletCache _newWalletCache;
        private readonly WalletCache _walletCache;
        private readonly DaemonWalletService _walletService;
        private readonly NotificationService _notificationService;

        public ObservableCollection<string> NetworkTypes { get; set; } = new ObservableCollection<string> { "Testnet" };
        public int SelectedNetworkTypeIndex { get; set; }

        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public string WalletName { get => _newWalletCache.WalletName; set { _newWalletCache.WalletName = value; ValidateCanContinue(); } }
        public bool Bootstrap { get => _newWalletCache.Bootstrap; set => _newWalletCache.Bootstrap = value; }


        private bool _canContinue = false;
        public bool CanContinue { get => _canContinue; set { _canContinue = value; OnPropertyChanged(nameof(CanContinue)); } }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public WalletDetailsViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache, WalletCache walletCache, DaemonWalletService walletService, NotificationService notificationService)
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

            CanContinue = true;
        }


        async Task CreateWalletAndContinue()
        {
            IsLoading = true;

            if (await _walletService.CreateWallet(_newWalletCache.WalletName, _newWalletCache.Mnemonic.Select(x => x).Aggregate((x, y) => x + " " + y), _newWalletCache.Password) == null)
            {
                _notificationService.DisplayError("An error occured while trying to create the wallet.");
                return;
            }

            _walletCache.Label = _newWalletCache.WalletName;
            _newWalletCache.Clear();

            _navigationServiceFactory.Create<WalletCreatedSuccessfullyViewModel>().Navigate();
        }
    }
}
