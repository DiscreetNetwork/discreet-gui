using Avalonia.Input;
using Avalonia.Interactivity;
using ReactiveUI;
using Services.Caches;
using Services.Daemon;
using Services.Daemon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using WPF.Factories.Navigation;
using WPF.Services;
using WPF.ViewModels.Common;

namespace WPF.Views.Account.Modals
{
    public class TransactionDetailsViewModel : ViewModelBase
    {
        private readonly TransactionDetailsCache _transactionDetailsCache;
        private readonly WalletService _walletService;
        private readonly NotificationService _notificationService;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        private TransactionHistoryElement _transaction;
        public TransactionHistoryElement Transaction { get => _transaction; set { _transaction = value; 
                OnPropertyChanged(nameof(Transaction));
                OnPropertyChanged(nameof(Amount));
                OnPropertyChanged(nameof(SentOrReceived));
            } }

        public bool IsSendTransaction { get; set; }
        public string Amount { get; set; }
        public string SentOrReceived { get; set; }
        public string Receiver { get; set; }
        public string Date { get; set; }
        public string TransactionId { get; set; }

        public TransactionDetailsViewModel(TransactionDetailsCache transactionDetailsCache, WalletService walletService, NotificationService notificationService, NavigationServiceFactory navigationServiceFactory)
        {
            _transactionDetailsCache = transactionDetailsCache;
            _walletService = walletService;
            _notificationService = notificationService;
            _navigationServiceFactory = navigationServiceFactory;

            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }

        async void OnActivated()
        {
            Transaction = await _walletService.GetTransaction(_transactionDetailsCache.Address, _transactionDetailsCache.TransactionId);
            if(Transaction is null)
            {
                _notificationService.Display("Failed to fetch the transaction");
                Dismiss();
                return;
            }

            IsSendTransaction = Transaction.SentAmount != 0;
            OnPropertyChanged(nameof(IsSendTransaction));

            Amount = IsSendTransaction ? Transaction.SentAmount.ToString() : Transaction.ReceivedAmount.ToString();
            OnPropertyChanged(nameof(Amount));

            SentOrReceived = IsSendTransaction ? "(Sent)" : "(Received)";
            OnPropertyChanged(SentOrReceived);

            Receiver = Transaction.Outputs.Any() ? Transaction.Outputs.Where(o => o.Address != _transactionDetailsCache.Address).FirstOrDefault()?.Address : "";
            OnPropertyChanged(nameof(Receiver));

            Date = new DateTime(Transaction.Timestamp).ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss");
            OnPropertyChanged(nameof(Date));

            TransactionId = Transaction.TxID;
            OnPropertyChanged(nameof(TransactionId));
        }

        void Dismiss() => _navigationServiceFactory.CreateModalNavigationService().Navigate();
    }
}
