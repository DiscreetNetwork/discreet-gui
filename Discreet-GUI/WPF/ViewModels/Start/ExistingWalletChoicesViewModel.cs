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
        ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }
        ReactiveCommand<Unit, Unit> NavigateOpenWalletFromFileCommand { get; set; }
        ReactiveCommand<Unit, Unit> NavigateRestoreWalletCommand { get; set; }

        public ExistingWalletChoicesViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<StartViewModel>().Navigate);

            NavigateOpenWalletFromFileCommand = ReactiveCommand.Create(() =>
            {
                //navigationServiceFactory.CreateModalNavigationService<AboutBootstrapViewModel>().Navigate();
                navigationServiceFactory.CreateStackNavigation<ExistingWalletChoicesViewModel, OpenWalletFromFileViewModel>().Navigate();
            });
            NavigateRestoreWalletCommand = ReactiveCommand.Create(() =>
            {

                //navigationServiceFactory.CreateModalNavigationService<AboutBootstrapViewModel>().Navigate();
                navigationServiceFactory.CreateStackNavigation<ExistingWalletChoicesViewModel, RestoreWalletViewModel>().Navigate();
            });
        }
    }
}
