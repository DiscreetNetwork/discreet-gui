using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Stores;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Start;

namespace Discreet_GUI.Views.Layouts
{
    public class DarkTitleBarLayoutWithBackButtonViewModel : TitleBarViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public DarkTitleBarLayoutWithBackButtonViewModel(NavigationServiceFactory navigationServiceFactory, ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore) : base(contentViewModel, windowSettingsStore)
        {
            _navigationServiceFactory = navigationServiceFactory;
        }

        public void NavigateBack()
        {
            _navigationServiceFactory.Create<StartViewModel>().Navigate();
            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }
    }
}
