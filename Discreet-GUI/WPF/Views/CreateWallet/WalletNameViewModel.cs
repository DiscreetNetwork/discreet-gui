using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using Services.Daemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Services.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.Views.Layouts;
using WPF.Views.Start;

namespace WPF.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class WalletNameViewModel : ViewModelBase
    {
        private readonly NewWalletCache _newWalletCache;
        private readonly WalletService _walletService;

        public ReactiveCommand<Unit, Unit> NavigateYourRecoveryPhraseViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public string WalletName { get => _newWalletCache.WalletName; set { _newWalletCache.WalletName = value; ValidateCanContinue(); } }


        private bool _canContinue = false;
        public bool CanContinue { get => _canContinue; set { _canContinue = value; OnPropertyChanged(nameof(CanContinue)); } }

        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        public WalletNameViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache, WalletService walletService)
        {
            _newWalletCache = newWalletCache;
            _walletService = walletService;
            NavigateYourRecoveryPhraseViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<YourRecoveryPhraseViewModel>().Navigate);
            NavigateBackCommand                     = ReactiveCommand.Create(navigationServiceFactory.Create<StartViewModel>().Navigate);

            ValidateCanContinue();
            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }


        async void OnActivated()
        {
            IsLoading = true;

            if (_newWalletCache.Mnemonic is null)
            {
                var mnemonic = await _walletService.GetMnemonic();
                _newWalletCache.Mnemonic = mnemonic.Value.Split(' ').Select(x => x).ToList();
            }

            IsLoading = false;
        }

        public void ValidateCanContinue()
        {
            if (string.IsNullOrWhiteSpace(WalletName))
            {
                CanContinue = false;
                return;
            }

            if(IsLoading)
            {
                CanContinue = false;
                return;
            }

            CanContinue = true;
        }
    }
}
