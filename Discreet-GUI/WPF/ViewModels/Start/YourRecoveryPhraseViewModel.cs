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
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class YourRecoveryPhraseViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateNextCommand { get; set; }
        public YourRecoveryPhraseViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateNextCommand = ReactiveCommand.Create(navigationServiceFactory.CreateStackNavigation<YourRecoveryPhraseViewModel, VerifyRecoveryPhraseViewModel>().Navigate);
        }
    }
}
