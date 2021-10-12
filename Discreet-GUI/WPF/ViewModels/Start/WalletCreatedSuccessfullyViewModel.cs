using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Start
{
    class WalletCreatedSuccessfullyViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateNextCommand { get; set; }
        public WalletCreatedSuccessfullyViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateNextCommand = ReactiveCommand.Create(() => { });
        }
    }
}
