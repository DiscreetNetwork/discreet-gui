using Avalonia.Input;
using Avalonia.Interactivity;
using ReactiveUI;
using Services;
using Services.Caches;
using Services.Daemon;
using Services.Daemon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Services;
using Discreet_GUI.ViewModels.Common;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Discreet_GUI.Views.Account.Modals
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

            Amount = $"{(IsSendTransaction ? DISTConverter.ToStringFormat(DISTConverter.Divide(Transaction.SentAmount)) : DISTConverter.ToStringFormat(DISTConverter.Divide(Transaction.ReceivedAmount)))} DIST";
            OnPropertyChanged(nameof(Amount));

            SentOrReceived = IsSendTransaction ? "(Sent)" : "(Received)";
            OnPropertyChanged(SentOrReceived);

            Receiver = Transaction.Outputs.Any() ? Transaction.Outputs.Where(o => o.Address != _transactionDetailsCache.Address).FirstOrDefault()?.Address : "";
            OnPropertyChanged(nameof(Receiver));

            Date = new DateTime(Transaction.Timestamp).ToString("dd-MM-yyyy HH:mm:ss");
            OnPropertyChanged(nameof(Date));

            TransactionId = Transaction.TxID;
            OnPropertyChanged(nameof(TransactionId));
        }

        void DisplayTransactionInExplorer()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // If no associated application/json MimeType is found xdg-open opens retrun error
                // but it tries to open it anyway using the console editor (nano, vim, other..)
                ShellExec($"xdg-open https://explorer.discreet.tools/transaction/details/{TransactionId}", waitForExit: false);
            }
            else
            {
                using Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"https://explorer.discreet.tools/transaction/details/{TransactionId}" : "open",
                    Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? $"https://explorer.discreet.tools/transaction/details/{TransactionId}" : "",
                    CreateNoWindow = true,
                    UseShellExecute = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                });
            }
        }

        private static void ShellExec(string cmd, bool waitForExit = true)
        {
            var escapedArgs = Regex.Replace(cmd, "(?=[`~!#&*()|;'<>])", "\\")
                .Replace("\"", "\\\\\\\"");

            using (var process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = "/bin/sh",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            ))
            {
                if (waitForExit)
                {
                    process.WaitForExit();
                }
            }
        }

        void Dismiss() => _navigationServiceFactory.CreateModalNavigationService().Navigate();
    }
}
