using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using ReactiveUI;
using System.Reactive.Concurrency;
using Discreet_GUI.Views.Layouts;
using Services.Daemon.Status;
using System.Reactive.Disposables;

namespace Discreet_GUI.Views.Modals
{
    [Layout(typeof(PurpleTitleBarLayoutViewModel))]
    public class LoadingSpinnerViewModel : ViewModelBase, IActivatableViewModel
    {
        private readonly DaemonCache _daemonCache;
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public ViewModelActivator Activator { get; set; }

        private string _peerState = string.Empty;
        public string PeerState { get => _peerState; set { _peerState = value; OnPropertyChanged(nameof(PeerState)); } }

        public LoadingSpinnerViewModel(DaemonCache daemonCache, NavigationServiceFactory navigationServiceFactory, WalletCache walletCache, DaemonStatusService statusService)
        {
            _daemonCache = daemonCache;
            _navigationServiceFactory = navigationServiceFactory;

            Activator = new ViewModelActivator();
            this.WhenActivated(d =>
            {
                if (!_daemonCache.DaemonStarted)
                {
                    _daemonCache.DaemonStartedChanged += DaemonStateChangedHandler;
                }

                Disposable.Create(() => _daemonCache.DaemonStartedChanged -= DaemonStateChangedHandler).DisposeWith(d);
            });
        }

        void DaemonStateChangedHandler()
        {
            if (_daemonCache.DaemonStarted)
            {
                _navigationServiceFactory.CloseDaemonStartupModal();
            }
        }
    }
}
