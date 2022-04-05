using ReactiveUI;
using Services.Daemon;
using Services.Daemon.Models;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF.Caches;
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
        public bool DisplayPassword { get => _displayPassword; set { _displayPassword = value; OnPropertyChanged(nameof(DisplayPassword)); } }
        private string _passwordCharacter = "*";
        
        public string PasswordCharacter { get => _passwordCharacter; set { _passwordCharacter = value; OnPropertyChanged(nameof(PasswordCharacter)); } }

        public string EnteredPassword { get; set; }

        public List<Wallet> LoadedWallets { get; set; }
        public Wallet SelectedWallet => LoadedWallets[SelectedWalletIndex];

        public List<WalletStatusData> WalletStatuses { get; set; }

        public WalletStatusData SelectedWalletStatus => WalletStatuses[SelectedWalletIndex];

        private int _selectedWalletIndex;
        public int SelectedWalletIndex { get => _selectedWalletIndex; set { _selectedWalletIndex = value; OnPropertyChanged(nameof(SelectedWallet)); OnPropertyChanged(nameof(SelectedWalletStatus));  } }

        public SelectWalletViewModel() { }

        public SelectWalletViewModel(NavigationServiceFactory navigationServiceFactory, WalletService walletService, NotificationService notificationService, WalletCache walletCache)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _walletService = walletService;
            _notificationService = notificationService;
            _walletCache = walletCache;
            var task = walletService.GetWallets();
            var wallets = task.Result;
            if(wallets is null)
            {
                notificationService.Display("Failed to load wallets");
            }
            else
            {
                LoadedWallets = wallets;
            }


            var statusesTask = walletService.GetWalletStatuses();
            WalletStatuses = statusesTask.Result;
        }


        void ToggleDisplayPasswordCommand()
        {
            DisplayPassword = !DisplayPassword;
            PasswordCharacter = DisplayPassword ? string.Empty : "*";
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
