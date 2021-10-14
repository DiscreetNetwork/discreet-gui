using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;

namespace WPF.ViewModels.Modals
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    class AboutBootstrapViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> ContinueCommand { get; set; }

        public AboutBootstrapViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            ContinueCommand = ReactiveCommand.Create(navigationServiceFactory.CreateModalNavigationService().Navigate);
        }
    }
}
