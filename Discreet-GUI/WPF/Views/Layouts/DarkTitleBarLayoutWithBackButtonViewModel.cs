using System;
using System.Collections.Generic;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Services.Navigation.Common;
using WPF.Stores;
using WPF.ViewModels.Common;
using WPF.Views.Start;

namespace WPF.Views.Layouts
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
