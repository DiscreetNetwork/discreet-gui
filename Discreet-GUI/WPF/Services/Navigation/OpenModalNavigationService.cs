using System;
using System.Collections.Generic;
using System.Text;
using WPF.Services.Navigation.Common;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;

namespace WPF.Services.Navigation
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
            _modalNavigationStore.CurrentModalViewModel = _createViewModel();
        }
    }
}
