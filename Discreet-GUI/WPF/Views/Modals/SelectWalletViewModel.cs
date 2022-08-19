using ReactiveUI;
using Services.Daemon;
using Services.Daemon.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Services.Caches;
using WPF.Factories.Navigation;
using WPF.Services;
using WPF.ViewModels.Common;
using WPF.Views.Account;
using WPF.Views.Layouts;
using WPF.Views.Layouts.Account;

namespace WPF.Views.Modals
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class SelectWalletViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletService _walletService;
        private readonly NotificationService _notificationService;
        private readonly WalletCache _walletCache;

        private bool _displayPassword = false;
        public bool DisplayPassword { get => _displayPassword; set { _displayPassword = value; OnPropertyChanged(nameof(DisplayPassword)); OnPropertyChanged(nameof(PasswordFontSize)); } }

        public string PasswordFontSize { get => DisplayPassword ? "14" : "10"; }

        private string _passwordCharacter = "●";
        public string PasswordCharacter { get => _passwordCharacter; set { _passwordCharacter = value; OnPropertyChanged(nameof(PasswordCharacter)); } }

        public string EnteredPassword { get; set; }

        private List<Wallet> _loadedWallets;
        public List<Wallet> LoadedWallets { get => _loadedWallets; set { _loadedWallets = value; OnPropertyChanged(nameof(LoadedWallets)); OnPropertyChanged(nameof(SelectedWallet)); } }
        public Wallet SelectedWallet => LoadedWallets[SelectedWalletIndex];

        private List<WalletStatusData> _walletStatuses;
        public List<WalletStatusData> WalletStatuses { get => _walletStatuses; set { _walletStatuses = value; OnPropertyChanged(nameof(WalletStatuses)); OnPropertyChanged(nameof(SelectedWalletStatus)); } }

        public WalletStatusData SelectedWalletStatus => SelectedWalletIndex == -1 ? null : WalletStatuses is null ? null : WalletStatuses[SelectedWalletIndex];

        private int _selectedWalletIndex;
        public int SelectedWalletIndex { get => _selectedWalletIndex; set { _selectedWalletIndex = value; OnPropertyChanged(nameof(SelectedWallet)); OnPropertyChanged(nameof(SelectedWalletStatus));  } }

        public SelectWalletViewModel() { }

        public SelectWalletViewModel(NavigationServiceFactory navigationServiceFactory, WalletService walletService, NotificationService notificationService, WalletCache walletCache)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _walletService = walletService;
            _notificationService = notificationService;
            _walletCache = walletCache;

            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }

        public async void OnActivated()
        {
            var wallets = await _walletService.GetWallets();
            if (wallets is null)
            {
                _notificationService.Display("Failed to load wallets");
            }
            else
            {
                LoadedWallets = wallets;
            }

            var statuses = await _walletService.GetWalletStatuses();
            if (statuses is null)
            {
                _notificationService.Display("Failed to load wallet statuses");
            }
            else
            {
                WalletStatuses = statuses;
            }
        }

        void ToggleDisplayPasswordCommand()
        {
            DisplayPassword = !DisplayPassword;
            PasswordCharacter = DisplayPassword ? string.Empty : "●";
        }


        /// <summary>
        /// Handler responsible for taking the appropiate action on the selected wallet, based on its state. Used to go forward with the 'Select Wallet' process
        /// </summary>
        /// <returns></returns>
        async Task Continue()
        {
            if(SelectedWalletStatus.Status == WalletStatus.UNLOADED)
            {
                var success = await _walletService.LoadWallet(LoadedWallets[SelectedWalletIndex].Label, EnteredPassword);
                if (!success)
                {
                    _notificationService.Display("Failed to load wallet, the passphrase might be wrong");
                    return;
                }
            }
            else if(SelectedWalletStatus.Status == WalletStatus.LOCKED)
            {
                var unlocked = await _walletService.UnlockWallet(LoadedWallets[SelectedWalletIndex].Label, EnteredPassword);
                if (!unlocked)
                {
                    _notificationService.Display("Wrong passphrase");
                    return;
                }
            }

            _walletCache.Label = LoadedWallets[SelectedWalletIndex].Label;
            _navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
            _navigationServiceFactory.CreateAccountNavigation<AccountLeftNavigationLayoutViewModel>().Navigate();
        }


        async Task UnlockWallet()
        {
            var unlocked = await _walletService.UnlockWallet(LoadedWallets[SelectedWalletIndex].Label, EnteredPassword);
            if(!unlocked)
            {
                _notificationService.Display("Wrong passphrase");
                return;
            }

            _navigationServiceFactory.Create<SelectWalletViewModel>().Navigate();
        }

        async Task LockWallet()
        {
            var locked = await _walletService.LockWallet(LoadedWallets[SelectedWalletIndex].Label);
            if (!locked)
            {
                _notificationService.Display("Failed to lock the wallet");
                return;
            }

            _navigationServiceFactory.Create<SelectWalletViewModel>().Navigate();
        }

        async Task LoadWallet()
        {
            var success = await _walletService.LoadWallet(LoadedWallets[SelectedWalletIndex].Label, EnteredPassword);
            if (!success)
            {
                _notificationService.Display("Failed to load wallet, the passphrase might be wrong");
                return;
            }

            _notificationService.Display("Loaded wallet");
            _navigationServiceFactory.Create<SelectWalletViewModel>().Navigate();
        }

        void JustNavigate()
        {
            _walletCache.Label = LoadedWallets[SelectedWalletIndex].Label;
            _navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
            _navigationServiceFactory.CreateAccountNavigation<AccountLeftNavigationLayoutViewModel>().Navigate();
        }
    }
}
