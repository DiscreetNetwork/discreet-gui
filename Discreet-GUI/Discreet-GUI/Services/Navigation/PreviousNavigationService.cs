using Discreet_GUI.Services.Navigation.Common;
using Discreet_GUI.Stores.Navigation;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Services.Navigation
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
