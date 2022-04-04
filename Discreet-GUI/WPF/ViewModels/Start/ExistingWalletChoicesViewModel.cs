using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Modals;

namespace WPF.ViewModels.Start
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel))]
    class ExistingWalletChoicesViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;

        ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }
        ReactiveCommand<Unit, Unit> NavigateRestoreWalletCommand { get; set; }

        public ExistingWalletChoicesViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<StartViewModel>().Navigate);

            

            NavigateRestoreWalletCommand = ReactiveCommand.Create(() =>
            {

                //navigationServiceFactory.CreateModalNavigationService<AboutBootstrapViewModel>().Navigate();
                navigationServiceFactory.CreateStackNavigation<ExistingWalletChoicesViewModel, RestoreWalletViewModel>().Navigate();
            });
            _navigationServiceFactory = navigationServiceFactory;
        }


        public void OpenWalletFromFile()
        {
            _navigationServiceFactory.Create<SelectWalletViewModel>().Navigate();
        }
    }
}
