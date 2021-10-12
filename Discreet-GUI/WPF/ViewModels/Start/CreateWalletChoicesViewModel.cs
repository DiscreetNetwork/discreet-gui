using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Services.Navigation.Common;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Start
{
    public class CreateWalletChoicesViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> BackCommand { get; set; }
        INavigationService _navigateYourRecoveryPhraseService;
        public CreateWalletChoicesViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            _navigateYourRecoveryPhraseService = navigationServiceFactory.CreateStackNavigation<CreateWalletChoicesViewModel, YourRecoveryPhraseViewModel>();
            BackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<StartViewModel>().Navigate);
        }

        public void NavigateYourRecoveryPhraseView()
        {
            _navigateYourRecoveryPhraseService.Navigate();
        }
    }
}
