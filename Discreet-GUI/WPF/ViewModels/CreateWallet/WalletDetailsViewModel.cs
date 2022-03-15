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
    class WalletDetailsViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateWalletCreatedViewCommand { get; set; }

        public WalletDetailsViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateWalletCreatedViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletCreatedSuccessfullyViewModel>().Navigate);
        }
    }
}
