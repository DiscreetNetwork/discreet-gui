using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.Stores.Navigation;
using WPF.ViewModels.Account;
using WPF.ViewModels.Common;
using WPF.ViewModels.Settings;

namespace WPF.ViewModels.Layouts.Account
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class AccountLeftNavigationLayoutViewModel : ViewModelBase
    {
        private readonly AccountNavigationStore _accountNavigationStore;
        public ViewModelBase CurrentViewModel => _accountNavigationStore.CurrentViewModel;

        public ReactiveCommand<Unit, Unit> NavigateAccountHomeCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountSendCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountReceiveCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountTransactionsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountSettingsCommand { get; set; }
        public ObservableCollection<bool> ButtonActiveStates { get; set; } = new ObservableCollection<bool>() { true, false, false, false, false };

        public decimal TotalBalance => WalletCache.TotalBalance;

        public WalletCache WalletCache { get; set; }

        public AccountLeftNavigationLayoutViewModel(AccountNavigationStore accountNavigationStore, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache)
        {
            WalletCache = walletCache;

            walletCache.TotalBalanceChanged += () => OnPropertyChanged(nameof(TotalBalance));

            _accountNavigationStore = accountNavigationStore;
            accountNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigateAccountHomeCommand              = ReactiveCommand.Create(() => 
            { 
                ResetButtonStates(); ButtonActiveStates[0] = true;
                navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
            });
            NavigateAccountSendCommand              = ReactiveCommand.Create(() => 
            { 
                ResetButtonStates(); ButtonActiveStates[1] = true;
                navigationServiceFactory.CreateAccountNavigation<AccountSendViewModel>().Navigate();
            });
            NavigateAccountReceiveCommand           = ReactiveCommand.Create(() => 
            { 
                ResetButtonStates(); ButtonActiveStates[2] = true;
                navigationServiceFactory.CreateAccountNavigation<AccountReceiveViewModel>().Navigate();
            });
            NavigateAccountTransactionsCommand      = ReactiveCommand.Create(() => 
            { 
                ResetButtonStates(); ButtonActiveStates[3] = true;
                navigationServiceFactory.CreateAccountNavigation<AccountTransactionsViewModel>().Navigate();
            });
            NavigateAccountSettingsCommand          = ReactiveCommand.Create(() => 
            { 
                ResetButtonStates(); ButtonActiveStates[4] = true;
                navigationServiceFactory.CreateAccountNavigation<SettingsViewModel>().Navigate();
            });
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        void ResetButtonStates()
        {
            for (int i = 0; i < ButtonActiveStates.Count; i++)
            {
                ButtonActiveStates[i] = false;
            }
        }
    }
}
