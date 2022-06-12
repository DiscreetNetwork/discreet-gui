using Services.Daemon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using Services.Daemon.Services;
using ReactiveUI;
using System.Reactive.Concurrency;

namespace WPF.Views.Modals
{
    public class LoadingSpinnerViewModel : ViewModelBase
    {
        private readonly WalletService _walletService;
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletCache _walletCache;
        private readonly StatusService _statusService;

        private string _peerState = string.Empty;
        public string PeerState { get => _peerState; set { _peerState = value; OnPropertyChanged(nameof(PeerState)); } }

        public LoadingSpinnerViewModel(WalletService walletService, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache, StatusService statusService)
        {
            _walletService = walletService;
            _navigationServiceFactory = navigationServiceFactory;
            _walletCache = walletCache;
            _statusService = statusService;

            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }

        public async void OnActivated()
        {
            while (true)
            {
                try
                {
                    var resp = await _statusService.GetHealth();

                    if (resp != null && resp.PeerState != null)
                    {
                        PeerState = resp.PeerState;
                        if (resp.PeerState.ToLower().Equals("normal")) break;
                    }
                }
                catch (Exception e)
                {
                    await Task.Delay(500);
                    continue;
                }
                

                await Task.Delay(100);
            }

            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }

    }
}
