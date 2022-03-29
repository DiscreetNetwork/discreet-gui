using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Start;

namespace WPF.ViewModels.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class WalletNameViewModel : ViewModelBase
    {
        private readonly NewWalletCache _newWalletCache;

        public ReactiveCommand<Unit, Unit> NavigateYourRecoveryPhraseViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public string WalletName { get => _newWalletCache.WalletName; set { _newWalletCache.WalletName = value; ValidateCanContinue(); } }

        public string WalletLocation { get => _newWalletCache.WalletLocation; set { _newWalletCache.WalletLocation = value; OnPropertyChanged(nameof(WalletLocation)); ValidateCanContinue(); } }

        private bool _canContinue = false;
        public bool CanContinue { get => _canContinue; set { _canContinue = value; OnPropertyChanged(nameof(CanContinue)); } }

        public WalletNameViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache)
        {
            _newWalletCache = newWalletCache;

            NavigateYourRecoveryPhraseViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<YourRecoveryPhraseViewModel>().Navigate);
            NavigateBackCommand                     = ReactiveCommand.Create(navigationServiceFactory.Create<StartViewModel>().Navigate);
        }

        public async Task OpenFolderDialogCommand()
        {
            var dlg = new OpenFolderDialog();
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var result = await dlg.ShowAsync(desktop.MainWindow);
                if (result != null)
                {
                    WalletLocation = result;
                }
            }

        }

        public void ValidateCanContinue()
        {
            if (string.IsNullOrWhiteSpace(WalletName))
            {
                CanContinue = false;
                return;
            }

            if(string.IsNullOrWhiteSpace(WalletLocation))
            {
                CanContinue = false;
                return;
            }

            CanContinue = true;
        }
    }
}
