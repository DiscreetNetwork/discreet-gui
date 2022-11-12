using Services.Daemon;
using Services.Daemon.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Services;
using Services.Daemon.Wallet.Requests;
using Services.Daemon.Transaction;
using Discreet_GUI.Services;

namespace Discreet_GUI.Views.Modals
{
    public class ConfirmViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly DaemonTransactionService _daemonTransactionService;
        private readonly SendTransactionCache _sendTransactionCache;
        private readonly NotificationService _notificationService;

        public string ReceiverAddress => $"{_sendTransactionCache.Receiver.Substring(0, 6)}...{_sendTransactionCache.Receiver.Substring(_sendTransactionCache.Receiver.Length - 6, 6)}";
        public decimal Amount => _sendTransactionCache.Amount;

         
        /// <summary>
        /// Can be used to display visuals, when the transaction request is processing
        /// </summary>
        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public ConfirmViewModel(NavigationServiceFactory navigationServiceFactory, DaemonTransactionService daemonTransactionService, SendTransactionCache sendTransactionCache, NotificationService notificationService)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _daemonTransactionService = daemonTransactionService;
            _sendTransactionCache = sendTransactionCache;
            _notificationService = notificationService;
        }

        public void CancelTransactionCommand()
        {
            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }

        public async Task ConfirmTransactionCommand()
        {
            IsLoading = true;

            var createdTransaction = await _daemonTransactionService.CreateTransaction(
                _sendTransactionCache.Sender,
                _sendTransactionCache.Receiver,
                DISTConverter.Multiply(_sendTransactionCache.Amount
            ));

            if(createdTransaction is null)
            {
                _notificationService.DisplayError("An error occured while trying to send the transaction.");
            }

            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }
    }
}
