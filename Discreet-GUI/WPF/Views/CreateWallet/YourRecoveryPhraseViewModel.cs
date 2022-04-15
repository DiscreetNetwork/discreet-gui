using Avalonia;
using Avalonia.Controls.Selection;
using ReactiveUI;
using Services.Daemon;
using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using Services.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.Views.CreateWallet;
using WPF.Views.Layouts;
using WPF.Views.Start;

namespace WPF.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class YourRecoveryPhraseViewModel : ViewModelBase
    {
        public ObservableCollection<string> GeneratedMnemonic { get; set; } = new ObservableCollection<string>();

        private bool _passphraseCopied = false;
        public bool PassphraseCopied { get => _passphraseCopied; set { _passphraseCopied = value; if (value) { PassphraseCopyContent = "Passphrase copied"; } else { PassphraseCopyContent = "Copy Passphrase"; } OnPropertyChanged(nameof(PassphraseCopied)); } }

        private string _passphraseCopyContent = "Copy Passphrase";
        private readonly NewWalletCache _newWalletCache;
        private readonly WalletService _walletService;

        public string PassphraseCopyContent { get => _passphraseCopyContent; set { _passphraseCopyContent = value; OnPropertyChanged(nameof(PassphraseCopyContent)); } }


        ReactiveCommand<Unit, Unit> NavigateNextCommand { get; set; }
        ReactiveCommand<Unit, Unit> CopyPassphraseCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }


        public YourRecoveryPhraseViewModel() { }

        public YourRecoveryPhraseViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache, WalletService walletService)
        {
            NavigateNextCommand = ReactiveCommand.Create(navigationServiceFactory.Create<VerifyRecoveryPhraseViewModel>().Navigate);
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletNameViewModel>().Navigate);

            CopyPassphraseCommand = ReactiveCommand.Create(() =>
            {
                PassphraseCopied = !PassphraseCopied;
                Application.Current.Clipboard.SetTextAsync(String.Join(" ", GeneratedMnemonic.Select(x => x)));
            });

            _newWalletCache = newWalletCache;
            _walletService = walletService;

            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }

        async void OnActivated()
        {
            GeneratedMnemonic = new ObservableCollection<string>(_newWalletCache.Mnemonic);
            OnPropertyChanged(nameof(GeneratedMnemonic));
        }
    }
}
