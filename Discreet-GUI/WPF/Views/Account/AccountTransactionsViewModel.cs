using Services.Daemon;
using Services.Daemon.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WPF.Caches;
using WPF.ViewModels.Common;

namespace WPF.Views.Account
{
    public class AccountTransactionsViewModel : ViewModelBase
    {
        List<AccountTransaction> Transactions { get; set; }

        public AccountTransactionsViewModel() { }

        public AccountTransactionsViewModel(WalletCache walletCache, WalletService walletService)
        {
            var accounts = walletCache.Accounts.ToList();
            var txs = new List<AccountTransaction>();
            foreach (var account in accounts)
            {
                var txTask = walletService.GetTransactionHistory(account.Address);
                var accountTxs = txTask.Result;

                if (accountTxs is null) continue;

                txs.AddRange(accountTxs.Select(x => new AccountTransaction(x.Timestamp, account.Address, x.SentAmount == 0 ? $"+{x.ReceivedAmount}" : $"-{x.SentAmount}")));
            }

            Transactions = txs.OrderByDescending(x => x.TransactionDate).ToList();
        }




        class AccountTransaction
        {
            public DateTime TransactionDate { get; set; }
            public string TimeFormatted => TransactionDate.TimeOfDay.ToString("hh':'mm");
            public string DateFormatted => TransactionDate.ToString("dd/MM/yyyy");
            public string ReceivingAccount { get; set; }

            /// <summary>
            /// This is a string, so we can append a "-" sign to it to display if the amount were received or sent
            /// </summary>
            public string Amount { get; set; }


            public AccountTransaction(long unix, string receiver, string amount)
            {
                TransactionDate = new DateTime(unix);
                ReceivingAccount = receiver;
                Amount = amount;
            }
        }
    }

    
}
