using Avalonia.Threading;
using Services.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Account
{
    public class AccountHomeViewModel : ViewModelBase
    {
        public ObservableCollection<MockItem> MockItems { get; set; } = new ObservableCollection<MockItem>
        {
            new MockItem
            {
               AccountLabel = "First account name",
               AccountBalance = 1127.50f
            },
            new MockItem
            {
               AccountLabel = "Second account name",
               AccountBalance = 7751.10f
            },
        };

        public WalletCache WalletCache { get; set; }
        public WalletManager WalletManager { get; set; }

        public ObservableCollection<WalletCache.Account> Accounts { get; set; } = new ObservableCollection<WalletCache.Account>();

        public decimal TotalBalance { get; set; }

        public AccountHomeViewModel(WalletCache walletCache, WalletManager walletManager)
        {
            WalletCache = walletCache;
            WalletManager = walletManager;

            walletCache.Accounts.ForEach(x => Accounts.Add(x));
            TotalBalance = WalletCache.TotalBalance;

            walletManager.OnWalletChange += WalletManager_OnWalletChange;

            _ = Task.Run(() => walletManager.Start(walletCache.Label)).ConfigureAwait(false);
        }

        private void WalletManager_OnWalletChange(object sender, WalletChangeEventArgs e)
        {
            // update GUI information...
            WalletCache.TotalBalance = e.cache.Wallet.TotalBalance;
            TotalBalance = e.cache.Wallet.TotalBalance;

            //WalletCache.Accounts.Zip(e.cache.Wallet.Addresses).ToList().ForEach(x => x.First.Balance = x.Second.Balance);
            foreach (var account in WalletCache.Accounts)
            {
                var address = e.cache.Wallet.Addresses.Where(x => x.Address == account.Address).FirstOrDefault();

                if (address != null)
                {
                    account.Balance = address.Balance;
                }
            }

            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(TotalBalance));
        }
    }




    public class MockItem
    {
        public string AccountLabel { get; set; }
        public float AccountBalance { get; set; }
    }
}
