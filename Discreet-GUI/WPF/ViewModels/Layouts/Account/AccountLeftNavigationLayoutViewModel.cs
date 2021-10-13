using System;
using System.Collections.Generic;
using System.Text;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts.Account
{
    class AccountLeftNavigationLayoutViewModel : ViewModelBase
    {
        private readonly AccountNavigationStore _accountNavigationStore;
        public ViewModelBase CurrentViewModel => _accountNavigationStore.CurrentViewModel;

        public AccountLeftNavigationLayoutViewModel(AccountNavigationStore accountNavigationStore)
        {
            _accountNavigationStore = accountNavigationStore;
            accountNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
