using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Text;
using System.Windows.Input;
using WPF.Stores;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    public class PurpleSystemMenuLayoutViewModel : ViewModelBase
    {
        private readonly WindowSettingsStore _windowSettingsStore;
        public ViewModelBase ContentViewModel { get; }

        public PurpleSystemMenuLayoutViewModel(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore)
        {
            ContentViewModel = contentViewModel;
            _windowSettingsStore = windowSettingsStore;
        }

        public void ToggleWindowStateHandler()
        {
            switch (_windowSettingsStore.CurrentWindowState)
            {
                case WindowState.Maximized:
                    _windowSettingsStore.CurrentWindowState = WindowState.Normal;
                    break;

                case WindowState.Normal:
                    _windowSettingsStore.CurrentWindowState = WindowState.Maximized;
                    break;
            }
        }
    }
}
