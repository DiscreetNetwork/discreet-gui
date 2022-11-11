using Services.Daemon;
using System.Threading.Tasks;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Services;
using Discreet_GUI.Stores;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Modals;
using Discreet_GUI.Views.Start;

namespace Discreet_GUI.Views.Layouts
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
                _notificationService.DisplayInformation("Failed to lock the wallet");
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
