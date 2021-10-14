using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;

namespace WPF.ViewModels.Start
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel), typeof(StartLayoutViewModel))]
    class ExistingWalletChoicesViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public ExistingWalletChoicesViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<StartViewModel>().Navigate);
        }
    }
}
