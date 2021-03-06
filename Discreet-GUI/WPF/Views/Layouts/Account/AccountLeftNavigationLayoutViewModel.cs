using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using Services.Caches;
using Services.Extensions;
using WPF.Factories.Navigation;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;
using WPF.Views.Account;
using WPF.Views.Settings;
using Services;
using WPF.Stores;

namespace WPF.Views.Layouts.Account
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class AccountLeftNavigationLayoutViewModel : ViewModelBase
    {
        private readonly AccountNavigationStore _accountNavigationStore;
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletCache _walletCache;
        private readonly UserPreferrencesStore _userPreferrencesStore;

        public ViewModelBase CurrentViewModel => _accountNavigationStore.CurrentViewModel;

        public ReactiveCommand<Unit, Unit> NavigateAccountHomeCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountSendCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountReceiveCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateAccountTransactionsCommand { get; set; }
        public ObservableCollection<bool> ButtonActiveStates { get; set; } = new ObservableCollection<bool>() { true, false, false, false, false, false };

        public ObservableCollectionEx<WalletCache.WalletAddress> Accounts => _walletCache.Accounts;
        public ulong TotalBalance => (ulong)Accounts.Sum(x => (long)x.Balance);
        public string WalletLabel => _walletCache.Label;

        public int NumberOfConnections => _walletCache.NumberOfConnections;

        public bool HideBalance => _userPreferrencesStore.HideBalance;

        public AccountLeftNavigationLayoutViewModel(AccountNavigationStore accountNavigationStore, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache, UserPreferrencesStore userPreferrencesStore)
        {
            _walletCache = walletCache;
            _userPreferrencesStore = userPreferrencesStore;
            Accounts.CollectionChanged += AccountsChanged;
            _walletCache.NumberOfConnectionsChanged += () => OnPropertyChanged(nameof(NumberOfConnections));

            _accountNavigationStore = accountNavigationStore;
            _navigationServiceFactory = navigationServiceFactory;
            accountNavigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));

            _userPreferrencesStore.HideBalanceChanged += () => OnPropertyChanged(nameof(HideBalance));

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
        }


        void NavigateSubmitIssueCommand()
        {
            ResetButtonStates();
            ButtonActiveStates[4] = true;
            _navigationServiceFactory.CreateAccountNavigation<SubmitIssueViewModel>().Navigate();
        }

        void NavigateAccountSettingsCommand() 
        {
            ResetButtonStates(); ButtonActiveStates[5] = true;
            _navigationServiceFactory.CreateAccountNavigation<SettingsViewModel>().Navigate();
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
