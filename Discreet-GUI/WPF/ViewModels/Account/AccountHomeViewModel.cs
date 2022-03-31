using WPF.Caches;
using WPF.ExtensionMethods;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Account
{
    public class AccountHomeViewModel : ViewModelBase
    {
        private readonly WalletCache _walletCache;

        public ObservableCollectionEx<WalletCache.WalletAddress> Accounts => _walletCache.Accounts;

        public decimal TotalBalance => _walletCache.TotalBalance;

        public AccountHomeViewModel() { }
        public AccountHomeViewModel(WalletCache walletCache)
        {
            _walletCache = walletCache;
            _walletCache.TotalBalanceChanged += () => OnPropertyChanged(nameof(TotalBalance));
        }
    }
}
