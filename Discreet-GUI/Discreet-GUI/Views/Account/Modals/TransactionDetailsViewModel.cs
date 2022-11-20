using ReactiveUI;
using Services;
using Services.Caches;
using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Services;
using Discreet_GUI.ViewModels.Common;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Avalonia;
using Services.Daemon.Wallet;
using Services.Daemon.Wallet.Models;
using System.Reactive.Disposables;

namespace Discreet_GUI.Views.Account.Modals
{
    public class TransactionDetailsViewModel : ViewModelBase, IActivatableViewModel
    {
        private readonly TransactionDetailsCache _transactionDetailsCache;
        private readonly DaemonWalletService _walletService;
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

        public ViewModelActivator Activator { get; set; }
        public TransactionDetailsViewModel(TransactionDetailsCache transactionDetailsCache, DaemonWalletService walletService, NotificationService notificationService, NavigationServiceFactory navigationServiceFactory)
        {
            _transactionDetailsCache = transactionDetailsCache;
            _walletService = walletService;
            _notificationService = notificationService;
            _navigationServiceFactory = navigationServiceFactory;

            Activator = new ViewModelActivator();
            this.WhenActivated(d =>
            {
                OnActivated();
                Disposable.Create(() => { }).DisposeWith(d);
            });
        }

        async void OnActivated()
        {
            Transaction = await _walletService.GetTransaction(_transactionDetailsCache.Address, _transactionDetailsCache.TransactionId);
            if(Transaction is null)
            {
                _notificationService.DisplayError("An error occured while trying to fetch the transaction.");
                Dismiss();
                return;
            }

            IsSendTransaction = Transaction.SentAmount != 0;
            OnPropertyChanged(nameof(IsSendTransaction));

            var amountDivided = IsSendTransaction ? DISTConverter.Divide(Transaction.SentAmount) : DISTConverter.Divide(Transaction.ReceivedAmount);
            var amountString = amountDivided is null ? "NaN" : $"{DISTConverter.ToStringFormat(amountDivided.Value)} DIST";
            Amount = amountString;
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

        async Task CopyTransactionHash()
        {
            await Application.Current.Clipboard.SetTextAsync(TransactionId);
            _notificationService.DisplayInformation("Copied transaction hash to clipboard");
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
