using System;
using System.Collections.Generic;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Services.Navigation.Common;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Start
{
    public class StartViewModel : ViewModelBase
    {
        INavigationService _navigateCreateWalletChoicesView;

        public StartViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            _navigateCreateWalletChoicesView = navigationServiceFactory.Create<CreateWalletChoicesViewModel>();
        }

        public void NavigateCreateWalletChoicesView() => _navigateCreateWalletChoicesView.Navigate();
    }
}
