using System;
using System.Collections.Generic;
using System.Text;
using WPF.Services.Navigation.Common;
using WPF.Stores.Navigation;
using WPF.ViewModels;

namespace WPF.Services.Navigation
{
    public class MainNavigationService : INavigationService
    {
        private readonly MainNavigationStore _mainNavigationStore;
        private readonly Func<ViewModelBase> _createViewModel;

        public MainNavigationService(MainNavigationStore mainNavigationStore, Func<ViewModelBase> createViewModel)
        {
            _mainNavigationStore = mainNavigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate() => _mainNavigationStore.CurrentViewModel = _createViewModel();
    }
}
