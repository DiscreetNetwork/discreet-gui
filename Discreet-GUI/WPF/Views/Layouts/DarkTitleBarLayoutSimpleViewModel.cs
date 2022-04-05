using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.Stores;
using WPF.ViewModels.Common;
using WPF.Views.Start;

namespace WPF.Views.Layouts
{
    class DarkTitleBarLayoutSimpleViewModel : TitleBarViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletCache _walletCache;

        public DarkTitleBarLayoutSimpleViewModel(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache) : base(contentViewModel, windowSettingsStore)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _walletCache = walletCache;
        }


        public void NavigateHomeCommand()
        {
            _walletCache.ClearCache();

            _navigationServiceFactory.Create<StartViewModel>().Navigate();
        }
    }
}
