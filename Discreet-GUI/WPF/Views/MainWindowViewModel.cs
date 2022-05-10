using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using WPF.Attributes;
using WPF.Stores;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;
using WPF.Views.DebugUtility;
using WPF.Views.Notifications;

namespace WPF.Views
{
    [AssemblyScanIgnore("This viewModel is manually registered in Startup")]
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly WindowSettingsStore _windowSettingsStore;
        private readonly MainNavigationStore _mainNavigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly MainDebugWindowViewModel _mainDebugWindowViewModel;

        public WindowState CurrentWindowState
        {
            get { return _windowSettingsStore.CurrentWindowState; }
            set
            {
                _windowSettingsStore.CurrentWindowState = value;
            }
        }
        public ViewModelBase CurrentViewModel => _mainNavigationStore.CurrentViewModel;
        public ViewModelBase CurrentModalViewModel => _modalNavigationStore.CurrentModalViewModel;

        public ViewModelBase NotificationContainerViewModel { get; set; }

        public MainWindowViewModel() { }
        public MainWindowViewModel(IConfiguration configuration, WindowSettingsStore windowSettingsStore, MainNavigationStore mainNavigationStore, ModalNavigationStore modalNavigationStore, NotificationContainerViewModel notificationContainerViewModel, MainDebugWindowViewModel mainDebugWindowViewModel)
        {
            _windowSettingsStore = windowSettingsStore;
            _windowSettingsStore.CurrentWindowStateChanged += OnCurrentWindowStateChanged;

            _mainNavigationStore = mainNavigationStore;
            _mainNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            _modalNavigationStore = modalNavigationStore;
            _modalNavigationStore.CurrentModalViewModelChanged += OnCurrentModalViewModelChanged;

            NotificationContainerViewModel = notificationContainerViewModel;
            _mainDebugWindowViewModel = mainDebugWindowViewModel;
            (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).Exit += (s, e) =>
            {
                Startup.Stop();

                if(configuration.GetValue<bool>("DaemonSettings:UseActivator"))
                {
                    Process daemonProcess = Process.GetProcessesByName("Discreet").FirstOrDefault();
                    if (daemonProcess != null)
                    {
                        daemonProcess.Kill();
                    }
                }
            };
        }

        private void OnCurrentWindowStateChanged() { OnPropertyChanged(nameof(CurrentWindowState)); }
        private void OnCurrentViewModelChanged() { OnPropertyChanged(nameof(CurrentViewModel)); }
        private void OnCurrentModalViewModelChanged() { OnPropertyChanged(nameof(CurrentModalViewModel)); }


#if DEBUG
        private MainDebugWindowView _debugView;
        void DisplayDebugProcess()
        {
            if(_debugView is null)
            {
                _debugView = new MainDebugWindowView();
                _debugView.DataContext = _mainDebugWindowViewModel;
                _debugView.Closing += (s, e) => _debugView = null; 
                _debugView.Show();
                return;
            }

            _debugView.Activate();
        }
#endif
    }
}
