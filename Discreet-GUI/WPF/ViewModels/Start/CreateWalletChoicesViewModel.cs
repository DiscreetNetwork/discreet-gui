using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Services.Navigation.Common;
using WPF.ViewModels.Common;
using WPF.ViewModels.CreateWallet;
using WPF.ViewModels.Layouts;

namespace WPF.ViewModels.Start
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel), typeof(StartLayoutViewModel))]
    public class CreateWalletChoicesViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> BackCommand { get; set; }
        ReactiveCommand<Unit, Unit> NavigateCreateWalletBootstrapChoicesCommand { get; set; }
        ReactiveCommand<Unit, Unit> NavigateWalletNameViewCommand { get; set; }

        public CreateWalletChoicesViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            BackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<StartViewModel>().Navigate);
            NavigateCreateWalletBootstrapChoicesCommand = ReactiveCommand.Create(navigationServiceFactory.Create<CreateWalletBootstrapChoicesViewModel>().Navigate);
            NavigateWalletNameViewCommand = ReactiveCommand.Create(navigationServiceFactory.CreateStackNavigation<CreateWalletChoicesViewModel, WalletNameViewModel>().Navigate);
        }
    }
}
