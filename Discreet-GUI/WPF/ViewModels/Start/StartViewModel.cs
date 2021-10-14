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
        INavigationService _navigateCreateWalletChoicesView;
        public ReactiveCommand<Unit, Unit> NavigateExistingWalletChoicesViewCommand { get; set; }
        public StartViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            _navigateCreateWalletChoicesView = navigationServiceFactory.Create<CreateWalletChoicesViewModel>();
            NavigateExistingWalletChoicesViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<ExistingWalletChoicesViewModel>().Navigate);
        }

        public void NavigateCreateWalletChoicesView() => _navigateCreateWalletChoicesView.Navigate();
    }
}
