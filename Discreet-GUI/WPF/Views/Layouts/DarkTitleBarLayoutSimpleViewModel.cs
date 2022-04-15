using ReactiveUI;
using Services.Daemon;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Services.Caches;
using WPF.Factories.Navigation;
using WPF.Services;
using WPF.Stores;
using WPF.ViewModels.Common;
using WPF.Views.Modals;
using WPF.Views.Start;

namespace WPF.Views.Layouts
{
    class DarkTitleBarLayoutSimpleViewModel : TitleBarViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly NotificationService _notificationService;
        private readonly WalletCache _walletCache;
        private readonly WalletService _walletService;

        public DarkTitleBarLayoutSimpleViewModel(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore, NavigationServiceFactory navigationServiceFactory, NotificationService notificationService, WalletCache walletCache, WalletService walletService) : base(contentViewModel, windowSettingsStore)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _notificationService = notificationService;
            _walletCache = walletCache;
            _walletService = walletService;
        }

        public void NavigateHomeCommand()
        {
            _walletCache.ClearCache();
            _navigationServiceFactory.Create<StartViewModel>().Navigate();
        }


        public async Task LockWallet()
        {
            if(!await _walletService.LockWallet(_walletCache.Label))
            {
                _notificationService.Display("Failed to lock the wallet");
                return;
            }

            _walletCache.ClearCache();
            _navigationServiceFactory.Create<SelectWalletViewModel>().Navigate();
        }

        public void GotoHome()
        {
            _walletCache.ClearCache();
            _navigationServiceFactory.Create<StartViewModel>().Navigate();
        }

        public void Exit()
        {
            _walletCache.ClearCache();
            base.CloseWindowHandler();
        }
    }
}
