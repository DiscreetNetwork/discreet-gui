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

namespace Discreet_GUI.Views.Layouts.Account
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    public class AccountLeftNavigationLayoutViewModel : ViewModelBase, IActivatableViewModel
    {
        private readonly AccountNavigationStore _accountNavigationStore;
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletCache _walletCache;
        private readonly DaemonCache _daemonCache;
        private readonly UserPreferrencesStore _userPreferrencesStore;

        public ViewModelBase CurrentViewModel => _accountNavigationStore.CurrentViewModel;

        public ObservableCollection<bool> ButtonActiveStates { get; set; } = new ObservableCollection<bool>() { true, false, false, false, false };

        private Avalonia.Media.Imaging.Bitmap _walletIdenticon;
        public Avalonia.Media.Imaging.Bitmap WalletIdenticon { get => _walletIdenticon; set { _walletIdenticon = value; OnPropertyChanged(nameof(WalletIdenticon)); } }

        public ObservableCollectionEx<WalletCache.WalletAddress> Accounts => _walletCache.Accounts;
        public ulong TotalBalance => (ulong)Accounts.Sum(x => (long)x.Balance);
        public string WalletLabel => _walletCache.Label;

        private string _daemonSyncLabel = "Daemon is synchronised";
        public string DaemonSyncLabel { get => _daemonSyncLabel; set { _daemonSyncLabel = value; OnPropertyChanged(nameof(DaemonSyncLabel)); } }

        public float SyncPercentage => _daemonCache.SyncPercentage;

        public int NumberOfConnections => _walletCache.NumberOfConnections;

        public bool HideBalance => _userPreferrencesStore.HideBalance;

        public ViewModelActivator Activator { get; set; }
        public AccountLeftNavigationLayoutViewModel(AccountNavigationStore accountNavigationStore, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache, DaemonCache daemonCache, UserPreferrencesStore userPreferrencesStore)
        {
            _walletCache = walletCache;
            _daemonCache = daemonCache;
            _userPreferrencesStore = userPreferrencesStore;
            Accounts.CollectionChanged += AccountsChangedHandler;

            _daemonCache.SyncPercentageChanged += UpdateSyncingStatus;
            _walletCache.NumberOfConnectionsChanged += NumberOfConnectionsChangedHandler;


            _accountNavigationStore = accountNavigationStore;
            _navigationServiceFactory = navigationServiceFactory;
            accountNavigationStore.CurrentViewModelChanged += CurrentAccountViewModelChangedHandler;

            _userPreferrencesStore.HideBalanceChanged += HideBalanceChangedHandler;

            Activator = new ViewModelActivator();
            this.WhenActivated(d =>
            {
                if (!string.IsNullOrWhiteSpace(_walletCache.EntropyHash))
                {
                    WalletIdenticon = JazziconEx.IdenticonToAvaloniaBitmap(160, _walletCache.EntropyHash);
                }
                _walletCache.EntropyHashChanged += () =>
                {
                    WalletIdenticon = JazziconEx.IdenticonToAvaloniaBitmap(160, _walletCache.EntropyHash);
                };

                Disposable.Create(() =>
                {
                    Accounts.CollectionChanged -= AccountsChangedHandler;
                    daemonCache.SyncPercentageChanged -= UpdateSyncingStatus;
                    walletCache.NumberOfConnectionsChanged -= NumberOfConnectionsChangedHandler;
                    accountNavigationStore.CurrentViewModelChanged -= CurrentAccountViewModelChangedHandler;
                    userPreferrencesStore.HideBalanceChanged -= HideBalanceChangedHandler;
                }).DisposeWith(d);
            });

            
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

        private void AccountsChangedHandler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalBalance));
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
