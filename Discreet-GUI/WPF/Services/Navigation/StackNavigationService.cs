using System;
using System.Collections.Generic;
using System.Text;
using WPF.Services.Navigation.Common;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;

namespace WPF.Services.Navigation
{
    class StackNavigationService : INavigationService
    {
        private readonly MainNavigationStore _mainNavigationStore;
        private readonly StackNavigationStore _stackNavigationStore;
        private readonly Func<ViewModelBase> _createPreviousViewModel;
        private readonly Func<ViewModelBase> _createTargetViewModel;

        public StackNavigationService(MainNavigationStore mainNavigationStore, StackNavigationStore stackNavigationStore, Func<ViewModelBase> createPreviousViewModel, Func<ViewModelBase> createTargetViewModel)
        {
            _mainNavigationStore = mainNavigationStore;
            _stackNavigationStore = stackNavigationStore;
            _createPreviousViewModel = createPreviousViewModel;
            _createTargetViewModel = createTargetViewModel;
        }

        public void Navigate()
        {
            _stackNavigationStore.PushViewModelBase(_createPreviousViewModel());
            _mainNavigationStore.CurrentViewModel = _createTargetViewModel();
        }
    }
}
