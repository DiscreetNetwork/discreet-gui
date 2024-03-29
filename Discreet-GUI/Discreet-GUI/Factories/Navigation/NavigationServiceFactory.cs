﻿using System;
using Discreet_GUI.Factories.ViewModel;
using Discreet_GUI.Services.Navigation;
using Discreet_GUI.Services.Navigation.Common;
using Discreet_GUI.Stores;
using Discreet_GUI.Stores.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Account;
using Discreet_GUI.Views.Layouts.Account;
using Discreet_GUI.Views.Settings;

namespace Discreet_GUI.Factories.Navigation
{
    /// <summary>
    /// This factory is responsible for constructing NavigationServices based on the specified Type of ViewModelBase
    /// </summary>
    public class NavigationServiceFactory
    {
        private readonly MainNavigationStore _mainNavigationStore;
        private readonly StackNavigationStore _stackNavigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly AccountNavigationStore _accountNavigationStore;
        private readonly NotificationStore _notificationStore;
        private readonly LayoutViewModelFactory _layoutViewModelFactory;

        public NavigationServiceFactory(MainNavigationStore mainNavigationStore, StackNavigationStore stackNavigationStore, ModalNavigationStore modalNavigationStore, AccountNavigationStore accountNavigationStore, NotificationStore notificationStore, LayoutViewModelFactory layoutViewModelFactory)
        {
            _mainNavigationStore = mainNavigationStore;
            _stackNavigationStore = stackNavigationStore;
            _modalNavigationStore = modalNavigationStore;
            _accountNavigationStore = accountNavigationStore;
            _notificationStore = notificationStore;
            _layoutViewModelFactory = layoutViewModelFactory;
        }

        public INavigationService Create<TViewModel>() where TViewModel : ViewModelBase
        {
            return new MainNavigationService(_mainNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
        }

        public INavigationService CreateStackNavigation<TFrom, TTarget>() where TFrom : ViewModelBase 
                                                                          where TTarget : ViewModelBase
        {
            return new StackNavigationService(_mainNavigationStore, _stackNavigationStore, _layoutViewModelFactory.Create<TFrom>, _layoutViewModelFactory.Create<TTarget>);
        }
        public INavigationService CreateStackNavigation()
        {
            return new PreviousNavigationService(_mainNavigationStore, _stackNavigationStore);
        }


        public INavigationService CreateModalNavigationService<TTarget>() where TTarget : ViewModelBase
        {
            return new OpenModalNavigationService(_modalNavigationStore, _layoutViewModelFactory.Create<TTarget>);
        }
        public INavigationService CreateModalNavigationService() => new CloseModalNavigationService(_modalNavigationStore);


        public INavigationService CreateAccountNavigation<TViewModel>() where TViewModel : ViewModelBase
        {
            if (typeof(TViewModel) == typeof(AccountLeftNavigationLayoutViewModel))     return new MainNavigationService(_mainNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(AccountHomeViewModel))                     return new AccountNavigationService(_accountNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(AccountSendViewModel))                     return new AccountNavigationService(_accountNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(AccountReceiveViewModel))                  return new AccountNavigationService(_accountNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(AccountTransactionsViewModel))             return new AccountNavigationService(_accountNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(SubmitIssueViewModel))                     return new AccountNavigationService(_accountNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(SettingsViewModel))                        return new AccountNavigationService(_accountNavigationStore, _layoutViewModelFactory.Create<TViewModel>);

            throw new InvalidOperationException();
        }

        public void SetDaemonStartupModal()
        {
            _modalNavigationStore.DaemonStartupModalViewModel = _layoutViewModelFactory.Create<Views.Modals.LoadingSpinnerViewModel>();
        }
        public void CloseDaemonStartupModal()
        {
            _modalNavigationStore.DaemonStartupModalViewModel = null;
        }
    }
}
