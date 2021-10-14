﻿using ReactiveUI;
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
    class ExistingWalletBootstrapChoicesViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }
        ReactiveCommand<Unit, Unit> NavigateOpenWalletFromFileCommand { get; set; }
        ReactiveCommand<Unit, Unit> NavigateRestoreWalletCommand { get; set; }

        public ExistingWalletBootstrapChoicesViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<ExistingWalletChoicesViewModel>().Navigate);
            NavigateOpenWalletFromFileCommand = ReactiveCommand.Create(navigationServiceFactory.CreateStackNavigation<ExistingWalletBootstrapChoicesViewModel, OpenWalletFromFileViewModel>().Navigate);
        }
    }
}
