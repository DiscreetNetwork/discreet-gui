using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using WPF.Stores;

namespace WPF.ViewModels.Common
{
    /// <summary>
    /// A base model that future custom title bar ViewModels can inherit from
    /// </summary>
    public abstract class TitleBarViewModelBase : ViewModelBase
    {
        public ViewModelBase ContentViewModel { get; }
        private readonly WindowSettingsStore _windowSettingsStore;

        public TitleBarViewModelBase(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore)
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
