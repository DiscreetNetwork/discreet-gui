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
        public ObservableCollection<MockItem> MockItems { get; set; } = new ObservableCollection<MockItem>(MockItem.Generate(24));

        

        public WalletCache WalletCache { get; set; }
        public WalletManager WalletManager { get; set; }

        //public ObservableCollection<WalletCache.Account> Accounts { get; set; } = new ObservableCollection<WalletCache.Account>();
        public List<WalletCache.Account> Accounts => WalletCache.Accounts; // New


        //public decimal TotalBalance { get; set; }
        public decimal TotalBalance => Accounts.Sum(x => x.Balance);

        public AccountHomeViewModel(WalletCache walletCache, WalletManager walletManager)
        {
            WalletCache = walletCache;
            WalletManager = walletManager;

            walletCache.Accounts.ForEach(x => Accounts.Add(x));
            //TotalBalance = WalletCache.TotalBalance;

            //walletManager.OnWalletChange += WalletManager_OnWalletChange;
            walletCache.AccountsChanged += OnAccountsChanged;

            _ = Task.Run(() => walletManager.Start(walletCache.Label)).ConfigureAwait(false);
        }

        // New
        void OnAccountsChanged()
        {
            OnPropertyChanged(nameof(Accounts));
            OnPropertyChanged(nameof(TotalBalance));
        }

        private void WalletManager_OnWalletChange(object sender, WalletChangeEventArgs e)
        {
            // update GUI information...
            WalletCache.TotalBalance = e.cache.Wallet.TotalBalance;
            //TotalBalance = e.cache.Wallet.TotalBalance;

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

        public static List<MockItem> Generate(int amount)
        {
            List<MockItem> items = new List<MockItem>();
            for (int i = 0; i < amount; i++)
            {
                items.Add(new MockItem() { AccountLabel = "Account name", AccountBalance = 7777.77f });
            }

            return items;
        }
    }
}
