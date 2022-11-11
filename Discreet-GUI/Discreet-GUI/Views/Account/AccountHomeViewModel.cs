using Avalonia;
using ReactiveUI;
using Services;
using Services.Caches;
using Services.Daemon;
using Services.Extensions;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Services;
using Discreet_GUI.Stores;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Views.Account
{
    public class AccountHomeViewModel : ViewModelBase
    {
        private readonly WalletCache _walletCache;
        private readonly UserPreferrencesStore _userPreferrencesStore;
        private readonly NotificationService _notificationService;
        private readonly WalletService _walletService;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public ObservableCollectionEx<WalletCache.WalletAddress> Accounts => _walletCache.Accounts;
        /*
        public ObservableCollectionEx<WalletCache.WalletAddress> Accounts { get; set; } = new ObservableCollectionEx<WalletCache.WalletAddress>()
        {
            new WalletCache.WalletAddress
            {
                Address = "1AxastPBd7LTHktMoMJvrAfG1h5cEtrT83SdGqzcW3NqRgJA3TzqFAr4wbgXPLBSuA9xhTPy44B84EYHFkrGSNwBLoSbAJh",
                Name = "taStealth",
                Balance = 350259,
                Type = WalletCache.AddressType.STEALTH,
                Synced = true,
                Identicon = JazziconEx.IdenticonToAvaloniaBitmap(160, "1AxastPBd7LTHkt")
            },
            new WalletCache.WalletAddress
            {
                Address = "1AxastPBd7LTHktMoMJvrAfG1h5cEtrT83SdGqzcW3NqRgJA3TzqFAr4wbgXPLBSuA9xhTPy44B84EYHFkrGSNwBLoSbAJh",
                Name = "taStealth",
                Balance = 350259,
                Type = WalletCache.AddressType.TRANSPARENT,
                Synced = false,
                Identicon = JazziconEx.IdenticonToAvaloniaBitmap(160, "1AxastPBd7LTHkt")
            }
        };
        */

        public ulong TotalBalance => (ulong)Accounts.Sum(x => (long)x.Balance);
        public bool HideBalance => _userPreferrencesStore.HideBalance;

        public AccountHomeViewModel() 
        {
        }

        public AccountHomeViewModel(WalletCache walletCache, UserPreferrencesStore userPreferrencesStore, NotificationService notificationService, WalletService walletService, NavigationServiceFactory navigationServiceFactory)
        {
            _walletCache = walletCache;
            _userPreferrencesStore = userPreferrencesStore;
            _notificationService = notificationService;
            _walletService = walletService;
            _navigationServiceFactory = navigationServiceFactory;
            Accounts.CollectionChanged += AccountsChanged;
        }

        private void AccountsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalBalance));
        }

        public async Task CopyAddress(string parameter)
        {
            await Application.Current.Clipboard.SetTextAsync(parameter);
            _notificationService.DisplayInformation("Copied address to clipboard");
        }

        void ToggleDisplayBalance()
        {
            _userPreferrencesStore.HideBalance = !_userPreferrencesStore.HideBalance;
            OnPropertyChanged(nameof(HideBalance));
        }

        public void DisplayAccountDetails(string accountId)
        {
            _walletCache.SelectedAccount = accountId;
            _navigationServiceFactory.CreateModalNavigationService<Modals.AccountDetailsViewModel>().Navigate();
        }

        public void DisplayCreateNewAccount()
        {
            _navigationServiceFactory.CreateModalNavigationService<Modals.CreateNewAccountViewModel>().Navigate();
        }
    }
}
