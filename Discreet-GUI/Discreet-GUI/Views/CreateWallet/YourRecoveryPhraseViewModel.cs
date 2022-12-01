using Avalonia;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;
using Services.Daemon.Wallet;
using System.Reactive.Disposables;

namespace Discreet_GUI.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class YourRecoveryPhraseViewModel : ViewModelBase, IActivatableViewModel
    {
        public ObservableCollection<string> GeneratedMnemonic { get; set; } = new ObservableCollection<string>();

        private bool _passphraseCopied = false;
        public bool PassphraseCopied { get => _passphraseCopied; set { _passphraseCopied = value; if (value) { PassphraseCopyContent = "Passphrase copied"; } else { PassphraseCopyContent = "Copy Passphrase"; } OnPropertyChanged(nameof(PassphraseCopied)); } }

        private string _passphraseCopyContent = "Copy Passphrase";
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly NewWalletCache _newWalletCache;

        public string PassphraseCopyContent { get => _passphraseCopyContent; set { _passphraseCopyContent = value; OnPropertyChanged(nameof(PassphraseCopyContent)); } }


        public ViewModelActivator Activator { get; set; }
        public YourRecoveryPhraseViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _newWalletCache = newWalletCache;

            Activator = new ViewModelActivator();
            this.WhenActivated(d =>
            {
                GeneratedMnemonic = new ObservableCollection<string>(_newWalletCache.Mnemonic);
                OnPropertyChanged(nameof(GeneratedMnemonic));
                Disposable.Create(() => { }).DisposeWith(d);
            });
        }

        void NavigateNextCommand()
        {
            _navigationServiceFactory.Create<VerifyRecoveryPhraseViewModel>().Navigate();
        }

        void NavigateBackCommand()
        {
            _navigationServiceFactory.Create<WalletNameViewModel>().Navigate();
        }

        void CopyPassphraseCommand()
        {
            PassphraseCopied = !PassphraseCopied;
            Application.Current.Clipboard.SetTextAsync(string.Join(" ", GeneratedMnemonic.Select(x => x)));
        }
    }
}
