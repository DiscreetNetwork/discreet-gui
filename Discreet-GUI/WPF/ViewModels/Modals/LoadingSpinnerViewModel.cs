using Services.Daemon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Modals
{
    public class LoadingSpinnerViewModel : ViewModelBase
    {
        private readonly WalletService _walletService;
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletCache _walletCache;

        public bool VisorStartupComplete => _walletCache.VisorStartupComplete;

        public LoadingSpinnerViewModel(WalletService walletService, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache)
        {
            _walletService = walletService;
            _navigationServiceFactory = navigationServiceFactory;
            _walletCache = walletCache;

            _ = Task.Run(async () =>
            {
                while(true)
                {
                    if (VisorStartupComplete) break;

                    await Task.Delay(100);
                }

                _navigationServiceFactory.CreateModalNavigationService().Navigate();
            });
        }
    }
}
