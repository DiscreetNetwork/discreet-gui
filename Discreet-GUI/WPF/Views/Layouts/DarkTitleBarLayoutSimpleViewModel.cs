using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.Stores;
using WPF.ViewModels.Common;
using WPF.Views.Start;

namespace WPF.Views.Layouts
{
    class DarkTitleBarLayoutSimpleViewModel : TitleBarViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateHomeCommand { get; set; }

        public DarkTitleBarLayoutSimpleViewModel(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore, NavigationServiceFactory navigationServiceFactory) : base(contentViewModel, windowSettingsStore)
        {
            NavigateHomeCommand = ReactiveCommand.Create(navigationServiceFactory.Create<StartViewModel>().Navigate);
        }
    }
}
