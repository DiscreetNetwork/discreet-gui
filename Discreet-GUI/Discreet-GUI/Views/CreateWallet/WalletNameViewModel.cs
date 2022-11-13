using ReactiveUI;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;
using Discreet_GUI.Views.Start;
using Services.Daemon.Wallet;
using System.Threading.Tasks;
using Discreet_GUI.Services;

namespace Discreet_GUI.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class WalletNameViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly NewWalletCache _newWalletCache;
        private readonly DaemonWalletService _walletService;
        private readonly NotificationService _notificationService;

        public string WalletName { get => _newWalletCache.WalletName; set { _newWalletCache.WalletName = value; ValidateCanContinue(); } }


        private bool _canContinue = false;
        public bool CanContinue { get => _canContinue; set { _canContinue = value; OnPropertyChanged(nameof(CanContinue)); } }

        private bool _isLoading = true;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public WalletNameViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache, DaemonWalletService walletService, NotificationService notificationService)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _newWalletCache = newWalletCache;
            _walletService = walletService;
            _notificationService = notificationService;
            ValidateCanContinue();
            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }


        async void OnActivated()
        {
            IsLoading = true;

            if (_newWalletCache.Mnemonic is null)
            {
                var mnemonic = await _walletService.GetMnemonic();
                if(mnemonic is null)
                {
                    IsLoading = false;
                    ValidateCanContinue();
                    _notificationService.DisplayError("An error occured while trying to generate the mnemonic.");
                    return;
                }

                _newWalletCache.Mnemonic = mnemonic.Value.Split(' ').Select(x => x).ToList();
            }

            IsLoading = false;
            ValidateCanContinue();
        }

        public void ValidateCanContinue()
        {
            if (string.IsNullOrWhiteSpace(WalletName))
            {
                CanContinue = false;
                return;
            }

            if(IsLoading)
            {
                CanContinue = false;
                return;
            }

            CanContinue = true;
        }


        async Task NavigateYourRecoveryPhraseViewCommand()
        {
            IsLoading = true;

            var wallets = await _walletService.GetWallets();
            if(wallets is null)
            {
                _notificationService.DisplayError("An error occured while trying to fetch existing wallets.");
                IsLoading = false;
                return;
            }

            if(wallets.Any(w => w.Label == WalletName))
            {
                _notificationService.DisplayError("A wallet with the specified label already exist.");
                IsLoading = false;
                return;
            }

            IsLoading = false;
            _navigationServiceFactory.Create<YourRecoveryPhraseViewModel>().Navigate();
        }

        void NavigateBackCommand()
        {
            _navigationServiceFactory.Create<StartViewModel>().Navigate();
        }
    }
}
