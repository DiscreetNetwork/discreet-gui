using Avalonia;
using ReactiveUI;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.ExtensionMethods;
using WPF.Services;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Account
{
    public class AccountHomeViewModel : ViewModelBase
    {
        private readonly WalletCache _walletCache;
        private readonly NotificationService _notificationService;

        public ObservableCollectionEx<WalletCache.WalletAddress> Accounts => _walletCache.Accounts;
        /* Mock data
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

        public decimal TotalBalance => Accounts.Sum(x => (decimal)x.Balance);


        public AccountHomeViewModel() 
        {
        }

        public AccountHomeViewModel(WalletCache walletCache, NotificationService notificationService)
        {
            _walletCache = walletCache;
            _notificationService = notificationService;
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
    }
}
