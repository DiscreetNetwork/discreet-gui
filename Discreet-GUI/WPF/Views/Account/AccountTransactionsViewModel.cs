using ReactiveUI;
using Services.Daemon;
using Services.Daemon.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;

namespace WPF.Views.Account
{
    public class AccountTransactionsViewModel : ViewModelBase
    {
        private readonly WalletCache _walletCache;
        private readonly WalletService _walletService;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        private List<AccountTransaction> _transactions;
        List<AccountTransaction> Transactions { get => _transactions; set { _transactions = value; OnPropertyChanged(nameof(Transactions)); } }

        public AccountTransactionsViewModel() { }

        public AccountTransactionsViewModel(WalletCache walletCache, WalletService walletService, NavigationServiceFactory navigationServiceFactory)
        {
            _walletCache = walletCache;
            _walletService = walletService;
            _navigationServiceFactory = navigationServiceFactory;

            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }

        public async void OnActivated()
        {
            var accounts = _walletCache.Accounts.ToList();
            var txs = new List<AccountTransaction>();
            foreach (var account in accounts)
            {
                var transactions = await _walletService.GetTransactionHistory(account.Address);

                if (transactions is null) continue;

                txs.AddRange(transactions.Select(x => new AccountTransaction(x.TxID, x.Timestamp, account.Address, x.SentAmount == 0 ? $"+{x.ReceivedAmount}" : $"-{x.SentAmount}")));
            }

            Transactions = txs.OrderByDescending(x => x.TransactionDate).ToList();
        }

        void DisplayTransactionDetails(string transactionId)
        {
            _navigationServiceFactory.CreateModalNavigationService<Modals.TransactionDetailsViewModel>().Navigate();
        }


        class AccountTransaction
        {
            public string TransactionId { get; set; }
            public DateTime TransactionDate { get; set; }
            public string TimeFormatted => TransactionDate.TimeOfDay.ToString("hh':'mm");
            public string DateFormatted => TransactionDate.ToString("dd/MM/yyyy");
            public string ReceivingAccount { get; set; }

            /// <summary>
            /// This is a string, so we can append a "-" sign to it to display if the amount were received or sent
            /// </summary>
            public string Amount { get; set; }


            public AccountTransaction(string transactionId, long unix, string receiver, string amount)
            {
                TransactionId = transactionId;
                TransactionDate = new DateTime(unix);
                ReceivingAccount = receiver;
                Amount = amount;
            }
        }
    }

    
}
