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
        private readonly WalletCache _walletCache;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        private TransactionHistoryElement _transaction;
        public TransactionHistoryElement Transaction { get => _transaction; set { _transaction = value; 
                OnPropertyChanged(nameof(Transaction));
                OnPropertyChanged(nameof(Amount));
        } }

        public string Amount => $"{(Transaction != null ? (Transaction.SentAmount != 0 ? Transaction.SentAmount : Transaction.ReceivedAmount) : "")} DIST";

        public TransactionDetailsViewModel(TransactionDetailsCache transactionDetailsCache, WalletService walletService, NotificationService notificationService, WalletCache walletCache, NavigationServiceFactory navigationServiceFactory)
        {
            _transactionDetailsCache = transactionDetailsCache;
            _walletService = walletService;
            _notificationService = notificationService;
            _walletCache = walletCache;
            _navigationServiceFactory = navigationServiceFactory;

            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }

        async void OnActivated()
        {
            Transaction = await _walletService.GetTransaction(_walletCache.Label, _transactionDetailsCache.TransactionId);
            if(Transaction is null)
            {
                _notificationService.Display("Failed to fetch the transaction");
                Dismiss();
                return;
            }
        }

        void Dismiss() => _navigationServiceFactory.CreateModalNavigationService().Navigate();
    }
}
