using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Services;
using Services.Daemon.Wallet;
using System.Reactive.Disposables;

namespace Discreet_GUI.Views.Account
{
    public class AccountTransactionsViewModel : ViewModelBase, IActivatableViewModel
    {
        private readonly WalletCache _walletCache;
        private readonly TransactionDetailsCache _transactionDetailsCache;
        private readonly DaemonWalletService _walletService;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        private List<AccountTransaction> _transactions;
        List<AccountTransaction> Transactions { get => _transactions; set { _transactions = value; OnPropertyChanged(nameof(Transactions)); } }

        public ViewModelActivator Activator { get; set; }
        public AccountTransactionsViewModel(WalletCache walletCache, TransactionDetailsCache transactionDetailsCache, DaemonWalletService walletService, NavigationServiceFactory navigationServiceFactory)
        {
            _walletCache = walletCache;
            _transactionDetailsCache = transactionDetailsCache;
            _walletService = walletService;
            _navigationServiceFactory = navigationServiceFactory;

            Activator = new ViewModelActivator();
            this.WhenActivated(d =>
            {
                OnActivated();
                Disposable.Create(() => { }).DisposeWith(d);
            });
        }

        public async void OnActivated()
        {
            var accounts = _walletCache.Accounts.ToList();
            var txs = new List<AccountTransaction>();
            foreach (var account in accounts)
            {
                var transactions = await _walletService.GetTransactionHistory(account.Address);

                if (transactions is null) continue;

                txs.AddRange(transactions.Select(x => new AccountTransaction(x.TxID, x.Timestamp, account.Address, account.Name, x.SentAmount == 0 ? $"+ {DISTConverter.ToStringFormat(DISTConverter.Divide(x.ReceivedAmount))}" : $"- {DISTConverter.ToStringFormat(DISTConverter.Divide(x.SentAmount))}")));
            }

            Transactions = txs.OrderByDescending(x => x.TransactionDate).ToList();
        }

        void DisplayTransactionDetails(AccountTransaction accountTransaction)
        {
            _transactionDetailsCache.TransactionId = accountTransaction.TransactionId;
            _transactionDetailsCache.Address = accountTransaction.AccountHash;
            _navigationServiceFactory.CreateModalNavigationService<Modals.TransactionDetailsViewModel>().Navigate();
        }


        class AccountTransaction
        {
            public string TransactionId { get; set; }
            public DateTime TransactionDate { get; set; }
            public string TimeFormatted => TransactionDate.TimeOfDay.ToString("hh':'mm");
            public string DateFormatted => TransactionDate.ToString("dd/MM/yyyy");
            public string AccountHash { get; set; }
            public string AccountName { get; set; }

            /// <summary>
            /// This is a string, so we can append a "-" sign to it to display if the amount were received or sent
            /// </summary>
            public string Amount { get; set; }


            public AccountTransaction(string transactionId, long unix, string accountHash, string accountName, string amount)
            {
                TransactionId = transactionId;
                TransactionDate = new DateTime(unix);
                AccountHash = accountHash;
                AccountName = accountName;
                Amount = amount;
            }
        }
    }

    
}
