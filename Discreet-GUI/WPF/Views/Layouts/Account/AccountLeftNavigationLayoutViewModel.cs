using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using WPF.Caches;
using WPF.ExtensionMethods;
using WPF.Factories.Navigation;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;
using WPF.Views.Account;
using WPF.Views.Settings;

namespace WPF.Views.Layouts.Account
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

        public ObservableCollectionEx<WalletCache.WalletAddress> Accounts => _walletCache.Accounts;
        public decimal TotalBalance => Accounts.Sum(x => (decimal)x.Balance);
        public string WalletLabel => _walletCache.Label;

        public int NumberOfConnections => _walletCache.NumberOfConnections;

        private readonly WalletCache _walletCache;

        public AccountLeftNavigationLayoutViewModel(AccountNavigationStore accountNavigationStore, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache)
        {
            _walletCache = walletCache;

            Accounts.CollectionChanged += AccountsChanged;
            _walletCache.NumberOfConnectionsChanged += () => OnPropertyChanged(nameof(NumberOfConnections));

            _accountNavigationStore = accountNavigationStore;
            accountNavigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));

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

        private void AccountsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalBalance));
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
