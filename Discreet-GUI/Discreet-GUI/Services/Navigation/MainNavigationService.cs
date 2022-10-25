using System;
using System.Collections.Generic;
using System.Text;
using Discreet_GUI.Services.Navigation.Common;
using Discreet_GUI.Stores.Navigation;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Services.Navigation
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
