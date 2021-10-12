using System;
using System.Collections.Generic;
using System.Text;
using WPF.Services.Navigation.Common;
using WPF.Stores.Navigation;

namespace WPF.Services.Navigation
{
    class CloseModalNavigationService : INavigationService
    {
        private readonly ModalNavigationStore _modalNavigationStore;

        public CloseModalNavigationService(ModalNavigationStore modalNavigationStore)
        {
            _modalNavigationStore = modalNavigationStore;
        }

        public void Navigate()
        {
            _modalNavigationStore.CurrentModalViewModel = null;
        }
    }
}
