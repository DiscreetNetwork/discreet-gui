using System;
using System.Collections.Generic;
using System.Text;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Stores.Navigation
{
    /// <summary>
    /// A store to hold the current Modal ViewModel, used to display notifications, popups etc.
    /// </summary>
    public class ModalNavigationStore
    {
        public event Action DaemonStartupModalViewModelChanged;
        private ViewModelBase _daemonStartupModalViewModel;
        public ViewModelBase DaemonStartupModalViewModel
        {
            get => _daemonStartupModalViewModel;
            set { _daemonStartupModalViewModel = value; OnDaemonStartupModalViewModelChanged(); }
        }
        void OnDaemonStartupModalViewModelChanged()
        {
            DaemonStartupModalViewModelChanged?.Invoke();
        }


        public event Action CurrentModalViewModelChanged;
        private ViewModelBase _currentModalViewModel;
        public ViewModelBase CurrentModalViewModel
        {
            get => _currentModalViewModel;
            set { _currentModalViewModel = value; OnCurrentModalViewModelChanged(); }
        }
        void OnCurrentModalViewModelChanged()
        {
            CurrentModalViewModelChanged?.Invoke();
        }
    }
}
