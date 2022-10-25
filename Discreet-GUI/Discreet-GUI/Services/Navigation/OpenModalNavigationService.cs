using System;
using System.Collections.Generic;
using System.Text;
using Discreet_GUI.Services.Navigation.Common;
using Discreet_GUI.Stores.Navigation;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Services.Navigation
{
    class OpenModalNavigationService : INavigationService
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly Func<ViewModelBase> _createViewModel;

        public OpenModalNavigationService(ModalNavigationStore modalNavigationStore, Func<ViewModelBase> createViewModel)
        {
            _modalNavigationStore = modalNavigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            var newViewModel = _createViewModel();
            if(_modalNavigationStore.CurrentModalViewModel != null)
            {
                if (_modalNavigationStore.CurrentModalViewModel.GetType() == newViewModel.GetType()) return;
            }

            _modalNavigationStore.CurrentModalViewModel = newViewModel;
        }
    }
}
