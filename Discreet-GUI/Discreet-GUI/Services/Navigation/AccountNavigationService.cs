using System;
using Discreet_GUI.Services.Navigation.Common;
using Discreet_GUI.Stores.Navigation;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Services.Navigation
{
    class AccountNavigationService : INavigationService
    {
        private readonly AccountNavigationStore _accountNavigationStore;
        private readonly Func<ViewModelBase> _createViewModel;

        public AccountNavigationService(AccountNavigationStore accountNavigationStore, Func<ViewModelBase> createViewModel)
        {
            _accountNavigationStore = accountNavigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _accountNavigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
