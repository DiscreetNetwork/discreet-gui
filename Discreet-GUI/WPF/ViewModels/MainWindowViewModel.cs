using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Attributes;
using WPF.Stores;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels
{
    [AssemblyScanIgnore("This viewModel is manually registered in Startup")]
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly WindowSettingsStore _windowSettingsStore;
        private readonly MainNavigationStore _mainNavigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;

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
        

        public MainWindowViewModel(WindowSettingsStore windowSettingsStore, MainNavigationStore mainNavigationStore, ModalNavigationStore modalNavigationStore)
        {
            _windowSettingsStore = windowSettingsStore;
            _windowSettingsStore.CurrentWindowStateChanged += OnCurrentWindowStateChanged;
            _mainNavigationStore = mainNavigationStore;
            _mainNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalNavigationStore = modalNavigationStore;
            _modalNavigationStore.CurrentModalViewModelChanged += OnCurrentModalViewModelChanged;

        }

        private void OnCurrentWindowStateChanged() { OnPropertyChanged(nameof(CurrentWindowState)); }
        private void OnCurrentViewModelChanged() { OnPropertyChanged(nameof(CurrentViewModel)); }
        private void OnCurrentModalViewModelChanged() { OnPropertyChanged(nameof(CurrentModalViewModel)); }
    }
}
