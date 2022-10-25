using System;
using System.Collections.Generic;
using System.Text;
using Discreet_GUI.Services.Navigation.Common;
using Discreet_GUI.Stores.Navigation;

namespace Discreet_GUI.Services.Navigation
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
