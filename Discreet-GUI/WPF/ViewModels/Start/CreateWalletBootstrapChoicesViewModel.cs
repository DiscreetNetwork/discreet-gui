﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Start
{
    class CreateWalletBootstrapChoicesViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> BackCommand { get; set; }

        public CreateWalletBootstrapChoicesViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            BackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<CreateWalletChoicesViewModel>().Navigate);
        }
    }
}
