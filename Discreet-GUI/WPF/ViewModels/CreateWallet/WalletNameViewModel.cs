using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Start;

namespace WPF.ViewModels.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class WalletNameViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> NavigateYourRecoveryPhraseViewCommand { get; set; }

        public WalletNameViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateYourRecoveryPhraseViewCommand = ReactiveCommand.Create(navigationServiceFactory.CreateStackNavigation<WalletNameViewModel, YourRecoveryPhraseViewModel>().Navigate);
        }
    }
}
