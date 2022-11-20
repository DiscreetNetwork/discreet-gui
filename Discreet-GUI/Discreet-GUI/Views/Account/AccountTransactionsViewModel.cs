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
using System.Threading.Tasks;

namespace Discreet_GUI.Views.Account
{
    public class AccountTransactionsViewModel : ViewModelBase, IActivatableViewModel
    {
        private readonly WalletCache _walletCache;
        private readonly TransactionDetailsCache _transactionDetailsCache;
        private readonly DaemonWalletService _walletService;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        private List<AccountTransaction> Transactions { get; set; }


        private List<AccountTransaction> _paginatedTransactions;
        public List<AccountTransaction> PaginatedTransactions { get => _paginatedTransactions; set { _paginatedTransactions = value; OnPropertyChanged(nameof(PaginatedTransactions)); } }

        private int _page = 0;
        private int Page { get => _page; set { _page = value; OnPropertyChanged(nameof(Page)); PaginationChanged(); } }

        private int PageSize { get; set; } = 8;

        public string PaginationStatusText { get 
        {
            if (Transactions is null || !Transactions.Any()) return "0-0";
            int from = (Page * PageSize) + 1;
            int to = ((Page + 1) * PageSize) > Transactions.Count ? Transactions.Count : (Page + 1) * PageSize;
            return $"{from} - {to}";
        } }

        public ViewModelActivator Activator { get; set; }
        public AccountTransactionsViewModel(WalletCache walletCache, TransactionDetailsCache transactionDetailsCache, DaemonWalletService walletService, NavigationServiceFactory navigationServiceFactory)
        {
            _walletCache = walletCache;
            _transactionDetailsCache = transactionDetailsCache;
            _walletService = walletService;
            _navigationServiceFactory = navigationServiceFactory;

            Activator = new ViewModelActivator();

            this.WhenActivated(async (d) =>
            {
                await OnActivated();
                Disposable.Create(() => { }).DisposeWith(d);
            });

            
        }

        public async Task OnActivated()
        {
            await Task.Delay(100);
            var accounts = _walletCache.Accounts.ToList();
            var txs = new List<AccountTransaction>();
            foreach (var account in accounts)
            {
                var transactions = await _walletService.GetTransactionHistory(account.Address);

                if (transactions is null) continue;

                txs.AddRange(transactions.Select(x =>
                {
                    var amountDivded = DISTConverter.Divide(x.ReceivedAmount);
                    string amountString = amountDivded is null ? "NaN" : DISTConverter.ToStringFormat(amountDivded.Value);
                    return new AccountTransaction(x.TxID, x.Timestamp, account.Address, account.Name, x.SentAmount == 0 ? $"+ {amountString}" : $"- {amountString}");
                }));
            }

            Transactions = txs.OrderByDescending(x => x.TransactionDate).ToList();

            PaginationChanged();
        }

        void DisplayTransactionDetails(AccountTransaction accountTransaction)
        {
            _transactionDetailsCache.TransactionId = accountTransaction.TransactionId;
            _transactionDetailsCache.Address = accountTransaction.AccountHash;
            _navigationServiceFactory.CreateModalNavigationService<Modals.TransactionDetailsViewModel>().Navigate();
        }


        #region PAGINATION
        void PaginationChanged()
        {
            PaginatedTransactions = Transactions.Skip(Page * PageSize).Take(PageSize).ToList();

            if(Page > 0)
            {
                PreviousEnabled = true;
            }
            else
            {
                PreviousEnabled = false;
            }

            if(Page < (int)Math.Ceiling((decimal)Transactions.Count / PageSize) - 1)
            {
                NextEnabled = true;
            }
            else
            {
                NextEnabled = false;
            }

            OnPropertyChanged(nameof(PaginationStatusText));
        }

        
        private bool _previousEnabled;
        public bool PreviousEnabled { get => _previousEnabled; set { _previousEnabled = value; OnPropertyChanged(nameof(PreviousEnabled)); } }
        void SkipPrevious()
        {
            Page = 0;
        }
        void Previous()
        {
            if(Page > 0)
            {
                Page--;
            }
        }
        private bool _nextEnabled;
        public bool NextEnabled { get => _nextEnabled; set { _nextEnabled = value; OnPropertyChanged(nameof(NextEnabled)); } }
        void Next()
        {
            if(Page < (int)Math.Ceiling((decimal)Transactions.Count / PageSize) - 1)
            {
                Page++;
            }
        }
        void SkipNext()
        {
            if (Page < (int)Math.Ceiling((decimal)Transactions.Count / PageSize) - 1)
            {
                Page = (int)Math.Ceiling((decimal)Transactions.Count / PageSize) - 1;
            }
        }
        #endregion



        public class AccountTransaction
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
