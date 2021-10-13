using System;
using System.Collections.Generic;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Services.Navigation.Common;
using WPF.Stores;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    public class DarkTitleBarLayoutWithBackButtonViewModel : TitleBarViewModelBase
    {
        INavigationService _previousNavigation;
        public DarkTitleBarLayoutWithBackButtonViewModel(NavigationServiceFactory navigationServiceFactory, ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore) : base(contentViewModel, windowSettingsStore)
        {
            _previousNavigation = navigationServiceFactory.CreateStackNavigation();
        }

        public void NavigateBack()
        {
            _previousNavigation.Navigate();
        }
    }
}
