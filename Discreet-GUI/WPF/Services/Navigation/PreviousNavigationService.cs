using System;
using System.Collections.Generic;
using System.Text;
using WPF.Services.Navigation.Common;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;

namespace WPF.Services.Navigation
{
    class PreviousNavigationService : INavigationService
    {
        private readonly MainNavigationStore _mainNavigationStore;
        private readonly StackNavigationStore _stackNavigationStore;

        public PreviousNavigationService(MainNavigationStore mainNavigationStore, StackNavigationStore stackNavigationStore)
        {
            _mainNavigationStore = mainNavigationStore;
            _stackNavigationStore = stackNavigationStore;
        }

        public void Navigate()
        {
            ViewModelBase previous = _stackNavigationStore.GetPreviousViewModelBase();
            _mainNavigationStore.CurrentViewModel = previous;
        }
    }
}
