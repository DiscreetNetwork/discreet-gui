using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Services.Navigation.Common;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;

namespace WPF.ViewModels.Start
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel), typeof(StartLayoutViewModel))]
    public class StartViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> NavigateCreateWalletChoicesViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateExistingWalletChoicesViewCommand { get; set; }
        public StartViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateCreateWalletChoicesViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<CreateWalletChoicesViewModel>().Navigate);
            NavigateExistingWalletChoicesViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<ExistingWalletChoicesViewModel>().Navigate);
        }
    }
}
