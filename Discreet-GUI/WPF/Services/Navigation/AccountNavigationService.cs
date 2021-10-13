using System;
using System.Collections.Generic;
using System.Text;
using WPF.Services.Navigation.Common;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;

namespace WPF.Services.Navigation
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
