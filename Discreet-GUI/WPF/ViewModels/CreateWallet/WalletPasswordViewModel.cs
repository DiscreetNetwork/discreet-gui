using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;

namespace WPF.ViewModels.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class WalletPasswordViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateWalletDetailsViewCommand { get; set; }

        public WalletPasswordViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateWalletDetailsViewCommand = ReactiveCommand.Create(navigationServiceFactory.CreateStackNavigation<WalletPasswordViewModel, WalletDetailsViewModel>().Navigate);
        }
    }
}
