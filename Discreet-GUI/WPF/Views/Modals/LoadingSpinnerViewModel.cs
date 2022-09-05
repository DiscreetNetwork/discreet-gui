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
        private readonly DaemonCache _daemonCache;
        private readonly WalletService _walletService;
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletCache _walletCache;
        private readonly StatusService _statusService;

        private string _peerState = string.Empty;
        public string PeerState { get => _peerState; set { _peerState = value; OnPropertyChanged(nameof(PeerState)); } }

        public LoadingSpinnerViewModel(DaemonCache daemonCache, WalletService walletService, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache, StatusService statusService)
        {
            _daemonCache = daemonCache;
            _walletService = walletService;
            _navigationServiceFactory = navigationServiceFactory;
            _walletCache = walletCache;
            _statusService = statusService;

            RxApp.MainThreadScheduler.Schedule(OnActivated);
        }

        public async void OnActivated()
        {
            if(!_daemonCache.DaemonStarted)
            {
                _daemonCache.DaemonStartedChanged += () =>
                {
                    if (_daemonCache.DaemonStarted)
                    {
                        _navigationServiceFactory.CreateModalNavigationService().Navigate();
                    }
                };
            }
        }

    }
}
