using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using WPF.Factories.ViewModel;
using WPF.Services.Navigation;
using WPF.Services.Navigation.Common;
using WPF.Stores.Navigation;
using WPF.ViewModels.Account;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts.Account;
using WPF.ViewModels.Start;

namespace WPF.Factories.Navigation
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
        private readonly LayoutViewModelFactory _layoutViewModelFactory;

        public NavigationServiceFactory(MainNavigationStore mainNavigationStore, StackNavigationStore stackNavigationStore, ModalNavigationStore modalNavigationStore, AccountNavigationStore accountNavigationStore, LayoutViewModelFactory layoutViewModelFactory)
        {
            _mainNavigationStore = mainNavigationStore;
            _stackNavigationStore = stackNavigationStore;
            _modalNavigationStore = modalNavigationStore;
            _accountNavigationStore = accountNavigationStore;
            _layoutViewModelFactory = layoutViewModelFactory;
        }

        public INavigationService Create<TViewModel>() where TViewModel : ViewModelBase
        {
            if (typeof(TViewModel) == typeof(StartViewModel))                   return new MainNavigationService(_mainNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(CreateWalletChoicesViewModel))     return new MainNavigationService(_mainNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(YourRecoveryPhraseViewModel))      return new MainNavigationService(_mainNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(VerifyRecoveryPhraseViewModel))    return new MainNavigationService(_mainNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(WalletCreatedSuccessfullyViewModel)) return new MainNavigationService(_mainNavigationStore, _layoutViewModelFactory.Create<TViewModel>);
            if (typeof(TViewModel) == typeof(CreateWalletBootstrapChoicesViewModel)) return new MainNavigationService(_mainNavigationStore, _layoutViewModelFactory.Create<TViewModel>);

            throw new InvalidOperationException();
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

            throw new InvalidOperationException();
        }
    }
}
