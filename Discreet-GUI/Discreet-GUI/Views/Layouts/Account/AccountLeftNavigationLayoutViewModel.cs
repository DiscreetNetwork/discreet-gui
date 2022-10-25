﻿using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using Services.Caches;
using Services.Extensions;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Stores.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Account;
using Discreet_GUI.Views.Settings;
using Services;
using Discreet_GUI.Stores;
using System.Reactive.Concurrency;

namespace Discreet_GUI.Views.Layouts.Account
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class AccountLeftNavigationLayoutViewModel : ViewModelBase
    {
        private readonly AccountNavigationStore _accountNavigationStore;
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletCache _walletCache;
        private readonly UserPreferrencesStore _userPreferrencesStore;

        public ViewModelBase CurrentViewModel => _accountNavigationStore.CurrentViewModel;

        public ObservableCollection<bool> ButtonActiveStates { get; set; } = new ObservableCollection<bool>() { true, false, false, false, false };

        public Avalonia.Media.Imaging.Bitmap WalletIdenticon { get; set; }

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

            _walletCache.EntropyHashChanged += () =>
            {
                WalletIdenticon = JazziconEx.IdenticonToAvaloniaBitmap(160, _walletCache.EntropyHash);
                OnPropertyChanged(nameof(WalletIdenticon));
            };
        }

        void NavigateHomeCommand()
        {
            ResetButtonStates();
            ButtonActiveStates[0] = true;
            _navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
        }

        void NavigateSendCommand()
        {
            ResetButtonStates();
            ButtonActiveStates[1] = true;
            _navigationServiceFactory.CreateAccountNavigation<AccountSendViewModel>().Navigate();
        }

        void NavigateTransactionsCommand()
        {
            ResetButtonStates();
            ButtonActiveStates[2] = true;
            _navigationServiceFactory.CreateAccountNavigation<AccountTransactionsViewModel>().Navigate();
        }

        void NavigateSubmitIssueCommand()
        {
            ResetButtonStates();
            ButtonActiveStates[3] = true;
            _navigationServiceFactory.CreateAccountNavigation<SubmitIssueViewModel>().Navigate();
        }

        void NavigateAccountSettingsCommand() 
        {
            ResetButtonStates(); 
            ButtonActiveStates[4] = true;
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