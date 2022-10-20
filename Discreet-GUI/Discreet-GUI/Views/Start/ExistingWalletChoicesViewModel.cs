using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;
using Discreet_GUI.Views.Modals;

namespace Discreet_GUI.Views.Start
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
