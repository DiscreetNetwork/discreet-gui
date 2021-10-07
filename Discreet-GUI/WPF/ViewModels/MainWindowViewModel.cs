using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Stores;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly WindowSettingsStore _windowSettingsStore;
        private readonly MainNavigationStore _mainNavigationStore;

        public WindowState CurrentWindowState => _windowSettingsStore.CurrentWindowState;
        public ViewModelBase CurrentViewModel => _mainNavigationStore.CurrentViewModel;

        public MainWindowViewModel(WindowSettingsStore windowSettingsStore, MainNavigationStore mainNavigationStore)
        {
            _windowSettingsStore = windowSettingsStore;
            _windowSettingsStore.CurrentWindowStateChanged += OnCurrentWindowStateChanged;
            _mainNavigationStore = mainNavigationStore;
            _mainNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            MaximizeWindowCommand = ReactiveCommand.Create(() =>
            {
                switch (WindowState)
                {
                    case WindowState.Maximized:
                        WindowState = WindowState.Normal;
                        break;

                    case WindowState.Normal:
                        WindowState = WindowState.Maximized;
                        break;
                }
            });
        }

        private void OnCurrentWindowStateChanged() { OnPropertyChanged(nameof(CurrentWindowState)); }
        private void OnCurrentViewModelChanged() { OnPropertyChanged(nameof(CurrentViewModel)); }
    }
}
