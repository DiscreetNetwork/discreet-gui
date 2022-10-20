using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;

namespace Discreet_GUI.Views.Modals
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
