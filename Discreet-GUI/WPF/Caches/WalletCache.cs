using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Caches
{
    public class WalletCache
    {
        

        public class Account
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public decimal Balance { get; set; }
        }

        public string Label { get; set; }
        public decimal TotalBalance { get; set; }


        public event Action AccountsChanged;

        List<Account> _accounts = new List<Account>();
        public List<Account> Accounts { get => _accounts; set { _accounts = value; OnAccountsChanged(); } }
        void OnAccountsChanged() { AccountsChanged?.Invoke(); }


        public void AddAccount(string name, string address, ulong balance)
        {
            Accounts.Add(new Account { Name = name, Address = address, Balance = balance });
        }
    }
}
