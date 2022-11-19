using ReactiveUI;
using System.Reactive;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;

namespace Discreet_GUI.Views.Modals
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class AboutBootstrapViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public AboutBootstrapViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            _navigationServiceFactory = navigationServiceFactory;
        }

        public void ContinueCommand()
        {
            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }
    }
}
