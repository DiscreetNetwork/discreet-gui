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
using WPF.Factories.Navigation;
using WPF.Services;
using WPF.ViewModels.Common;

namespace WPF.Views.Account
{
    public class AccountHomeViewModel : ViewModelBase
    {
        private readonly WalletCache _walletCache;
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
                Synced = true
            },
            new WalletCache.WalletAddress
            {
                Address = "1AxastPBd7LTHktMoMJvrAfG1h5cEtrT83SdGqzcW3NqRgJA3TzqFAr4wbgXPLBSuA9xhTPy44B84EYHFkrGSNwBLoSbAJh",
                Name = "taStealth",
                Balance = 350259,
                Type = WalletCache.AddressType.TRANSPARENT,
                Synced = false
            }
        };
        */

        public ulong TotalBalance => (ulong)Accounts.Sum(x => (long)x.Balance);


        public AccountHomeViewModel() 
        {
        }

        public AccountHomeViewModel(WalletCache walletCache, NotificationService notificationService, WalletService walletService, NavigationServiceFactory navigationServiceFactory)
        {
            _walletCache = walletCache;
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
            _notificationService.Display("Copied address to clipboard");
        }

        public void DisplayAccountDetails(string accountName)
        {
            _navigationServiceFactory.CreateModalNavigationService<Modals.AccountDetailsViewModel>().Navigate();
        }

        public void DisplayCreateNewAccount()
        {
            _navigationServiceFactory.CreateModalNavigationService<Modals.CreateNewAccountViewModel>().Navigate();
        }
    }
}
