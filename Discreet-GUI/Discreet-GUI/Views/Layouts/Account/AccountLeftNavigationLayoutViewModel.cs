using System.Collections.ObjectModel;
using System.Linq;
using Services.Caches;
using Services.Extensions;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Stores.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Account;
using Discreet_GUI.Views.Settings;
using Discreet_GUI.Stores;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Services.Daemon.Wallet;
using Discreet_GUI.Services;
using System;
using System.Text;
using Services.Daemon.Status;

namespace Discreet_GUI.Views.Layouts.Account
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    public class AccountLeftNavigationLayoutViewModel : ViewModelBase, IActivatableViewModel
    {
        private readonly AccountNavigationStore _accountNavigationStore;
        private readonly NotificationService _notificationService;
        private readonly DaemonStatusService _daemonStatusService;
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly DaemonWalletService _walletService;
        private readonly WalletCache _walletCache;
        private readonly DaemonCache _daemonCache;
        private readonly UserPreferrencesStore _userPreferrencesStore;

        public ViewModelBase CurrentViewModel => _accountNavigationStore.CurrentViewModel;

        public ObservableCollection<bool> ButtonActiveStates { get; set; } = new ObservableCollection<bool>() { true, false, false, false, false };

        private Avalonia.Media.Imaging.Bitmap _walletIdenticon;
        public Avalonia.Media.Imaging.Bitmap WalletIdenticon { get => _walletIdenticon; set { _walletIdenticon = value; OnPropertyChanged(nameof(WalletIdenticon)); } }

        public ulong TotalBalance => (ulong)_walletCache.Accounts.Sum(x => (long)x.Balance);
        public string WalletLabel => _walletCache.Label;

        private string _daemonSyncLabel = "Daemon is synchronised";
        public string DaemonSyncLabel { get => _daemonSyncLabel; set { _daemonSyncLabel = value; OnPropertyChanged(nameof(DaemonSyncLabel)); } }

        public float SyncPercentage => _daemonCache.SyncPercentage;

        public int NumberOfConnections => _walletCache.NumberOfConnections;

        public bool HideBalance => _userPreferrencesStore.HideBalance;

        public bool WalletLoaded { get; set; }

        public ViewModelActivator Activator { get; set; }
        public AccountLeftNavigationLayoutViewModel(AccountNavigationStore accountNavigationStore, NotificationService notificationService, DaemonStatusService daemonStatusService, NavigationServiceFactory navigationServiceFactory, DaemonWalletService walletService, WalletCache walletCache, DaemonCache daemonCache, UserPreferrencesStore userPreferrencesStore)
        {
            _notificationService = notificationService;
            _daemonStatusService = daemonStatusService;
            _navigationServiceFactory = navigationServiceFactory;
            _walletService = walletService;

            _walletCache = walletCache;
            _walletCache.NumberOfConnectionsChanged += NumberOfConnectionsChangedHandler;

            _daemonCache = daemonCache;
            _daemonCache.SyncPercentageChanged += UpdateSyncingStatus;

            _userPreferrencesStore = userPreferrencesStore;
            _userPreferrencesStore.HideBalanceChanged += HideBalanceChangedHandler;

            _walletCache.BalanceChanged += BalanceChangedHandler;

            _accountNavigationStore = accountNavigationStore;
            accountNavigationStore.CurrentViewModelChanged += CurrentAccountViewModelChangedHandler;

            Activator = new ViewModelActivator();
            this.WhenActivated(async (d) =>
            {
                await Task.Delay(100);
                await LoadWalletData();

                if (!string.IsNullOrWhiteSpace(_walletCache.EntropyHash))
                {
                    WalletIdenticon = JazziconEx.IdenticonToAvaloniaBitmap(160, _walletCache.EntropyHash);
                }

                Disposable.Create(() =>
                {
                    _walletCache.BalanceChanged -= BalanceChangedHandler;
                    daemonCache.SyncPercentageChanged -= UpdateSyncingStatus;
                    walletCache.NumberOfConnectionsChanged -= NumberOfConnectionsChangedHandler;
                    accountNavigationStore.CurrentViewModelChanged -= CurrentAccountViewModelChangedHandler;
                    userPreferrencesStore.HideBalanceChanged -= HideBalanceChangedHandler;
                }).DisposeWith(d);
            });

            
        }

        async Task LoadWalletData()
        {
            var _lbl = _walletCache.Label;
            _walletCache.ClearCache();
            _walletCache.Label = _lbl;

            var walletToFind = await _walletService.GetWallet(_walletCache.Label);
            if (walletToFind == null)
            {
                _notificationService.DisplayError("Could not find the selected wallet, please return to the main menu and try again.");
                return;
            }

            _walletCache.EntropyHash = BitConverter.ToString(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(walletToFind.EntropyChecksum)));
            _walletCache.LastSeenHeight = walletToFind.LastSeenHeight;

            //var syncStatus = await _walletService.GetWalletHeight(_walletCache.Label);
            //_walletCache.Synced = syncStatus.Synced;
            //_walletCache.LastSeenHeight = syncStatus.Height;

            var addresses = await _walletService.GetWalletAddresses(_walletCache.Label);
            foreach (var a in addresses)
            {
                _walletCache.AddAccount(a.Name, a.Address, a.Balance, a.Type == 0 ? WalletCache.AddressType.STEALTH : WalletCache.AddressType.TRANSPARENT);
            }

            Task t1 = Task.Run(async () =>
            {
                var numberOfConnections = await _daemonStatusService.GetNumConnections();
                if (numberOfConnections is null) return;

                var previous = _walletCache.NumberOfConnections;
                if (numberOfConnections != previous) _walletCache.NumberOfConnections = numberOfConnections.Value;
            });

            Task t2 = Task.Run(async () =>
            {
                bool balanceChanged = false;
                foreach (var address in _walletCache.Accounts)
                {
                    var fetchedBalance = await _walletService.GetBalance(address.Address);
                    if (fetchedBalance == null)
                    {
                        continue;
                    }

                    if (address.Balance != fetchedBalance)
                    {
                        address.Balance = fetchedBalance.Value;
                        balanceChanged = true;
                    }
                }
                if (balanceChanged) _walletCache.NotifyBalanceChanged();
            });

            Task t3 = Task.Run(async () =>
            {
                foreach (var address in _walletCache.Accounts)
                {
                    var addressState = await _walletService.GetAddressHeight(address.Address);
                    if (addressState is null)
                    {
                        continue;
                    }

                    if (address.Height != addressState.Height) address.Height = addressState.Height;
                    if (address.Syncer != addressState.Syncer) address.Syncer = addressState.Syncer;
                    if (address.Synced != addressState.Synced) address.Synced = addressState.Synced;
                }
            });

            Task t4 = Task.Run(async () =>
            {
                var walletState = await _walletService.GetWalletHeight(_walletCache.Label);
                if (walletState is null)
                {
                    return;
                }

                if (_walletCache.LastSeenHeight != walletState.Height) _walletCache.LastSeenHeight = walletState.Height;
                if (_walletCache.Synced != walletState.Synced) _walletCache.Synced = walletState.Synced;
            });

            await Task.WhenAll(t1, t2, t3, t4);

            WalletLoaded = true;
            OnPropertyChanged(nameof(WalletLoaded));
        }

        void BalanceChangedHandler()
        {
            OnPropertyChanged(nameof(TotalBalance));
        }

        void UpdateSyncingStatus()
        {
            OnPropertyChanged(nameof(SyncPercentage));

            if(SyncPercentage <= 0.25f)
            {
                DaemonSyncLabel = $"Fetching block headers: {_daemonCache.SyncFrom} -> {_daemonCache.SyncTo}";
            }
            else if(SyncPercentage <= 0.99)
            {
                DaemonSyncLabel = $"Syncing blocks: {_daemonCache.SyncFrom} -> {_daemonCache.SyncTo}";
            }
            else if(SyncPercentage == 1f)
            {
                DaemonSyncLabel = "Daemon is synchronized";
            }
        }

        void NavigateHomeCommand()
        {
            ResetButtonStates();
            ButtonActiveStates[0] = true;
            _navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
        }

        void NavigateSendCommand()
        {
            ResetButtonStates();
            ButtonActiveStates[1] = true;
            _navigationServiceFactory.CreateAccountNavigation<AccountSendViewModel>().Navigate();
        }

        void NavigateTransactionsCommand()
        {
            ResetButtonStates();
            ButtonActiveStates[2] = true;
            _navigationServiceFactory.CreateAccountNavigation<AccountTransactionsViewModel>().Navigate();
        }

        void NavigateSubmitIssueCommand()
        {
            ResetButtonStates();
            ButtonActiveStates[3] = true;
            _navigationServiceFactory.CreateAccountNavigation<SubmitIssueViewModel>().Navigate();
        }

        void NavigateAccountSettingsCommand() 
        {
            ResetButtonStates(); 
            ButtonActiveStates[4] = true;
            _navigationServiceFactory.CreateAccountNavigation<SettingsViewModel>().Navigate();
        }

        private void NumberOfConnectionsChangedHandler()
        {
            OnPropertyChanged(nameof(NumberOfConnections));
        }
        private void CurrentAccountViewModelChangedHandler()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private void HideBalanceChangedHandler()
        {
            OnPropertyChanged(nameof(HideBalance));
        }

        void ResetButtonStates()
        {
            for (int i = 0; i < ButtonActiveStates.Count; i++)
            {
                ButtonActiveStates[i] = false;
            }
        }
    }
}
