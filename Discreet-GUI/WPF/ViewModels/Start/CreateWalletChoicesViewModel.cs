using System;
using System.Collections.Generic;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Services.Navigation.Common;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Start
{
    public class CreateWalletChoicesViewModel : ViewModelBase
    {
        INavigationService _navigateYourRecoveryPhraseService;
        public CreateWalletChoicesViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            _navigateYourRecoveryPhraseService = navigationServiceFactory.Create<YourRecoveryPhraseViewModel>();
        }

        public void NavigateYourRecoveryPhraseView()
        {
            _navigateYourRecoveryPhraseService.Navigate();
        }
    }
}
