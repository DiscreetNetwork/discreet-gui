using ReactiveUI;
using System.Reactive;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;
using Discreet_GUI.Views.Modals;

namespace Discreet_GUI.Views.Start
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel))]
    class ExistingWalletChoicesViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public ExistingWalletChoicesViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            _navigationServiceFactory = navigationServiceFactory;
        }


        public void NavigateBackCommand()
        {
            _navigationServiceFactory.Create<StartViewModel>().Navigate();
        }

        public void NavigateRestoreWalletCommand()
        {
            _navigationServiceFactory.CreateStackNavigation<ExistingWalletChoicesViewModel, RestoreWalletViewModel>().Navigate();
        }

        public void OpenWalletFromFile()
        {
            _navigationServiceFactory.Create<SelectWalletViewModel>().Navigate();
        }
    }
}
