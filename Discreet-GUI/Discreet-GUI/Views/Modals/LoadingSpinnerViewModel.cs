﻿using Services.Daemon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Services.Daemon.Services;
using ReactiveUI;
using System.Reactive.Concurrency;
using Discreet_GUI.Views.Layouts;

namespace Discreet_GUI.Views.Modals
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel))]
    public class LoadingSpinnerViewModel : ViewModelBase
    {
        private readonly DaemonCache _daemonCache;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        private string _peerState = string.Empty;
        public string PeerState { get => _peerState; set { _peerState = value; OnPropertyChanged(nameof(PeerState)); } }

        public LoadingSpinnerViewModel(DaemonCache daemonCache, WalletService walletService, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache, StatusService statusService)
        {
            _daemonCache = daemonCache;
            _navigationServiceFactory = navigationServiceFactory;

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
                        _navigationServiceFactory.CloseDaemonStartupModal();
                    }
                };
            }
        }

    }
}