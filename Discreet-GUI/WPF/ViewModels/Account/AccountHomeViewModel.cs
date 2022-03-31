using System.Linq;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.ExtensionMethods;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Account
{
    public class AccountHomeViewModel : ViewModelBase
    {
        private readonly WalletCache _walletCache;

        public ObservableCollectionEx<WalletCache.WalletAddress> Accounts => _walletCache.Accounts;

        public decimal TotalBalance => Accounts.Sum(x => (decimal)x.Balance);

        public AccountHomeViewModel() { }
        public AccountHomeViewModel(WalletCache walletCache)
        {
            _walletCache = walletCache;

            Accounts.CollectionChanged += AccountsChanged;
        }

        private void AccountsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalBalance));
        }
    }
}
