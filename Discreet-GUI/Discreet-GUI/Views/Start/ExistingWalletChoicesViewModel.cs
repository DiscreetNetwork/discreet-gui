using ReactiveUI;
using System.Reactive;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;
using Discreet_GUI.Views.Modals;
using System.Reflection;

namespace Discreet_GUI.Views.Start
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel))]
    class ExistingWalletChoicesViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        public string CurrentVersion { get => $"Version: {Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Minor}.{Assembly.GetExecutingAssembly().GetName().Version.Build}"; }

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
