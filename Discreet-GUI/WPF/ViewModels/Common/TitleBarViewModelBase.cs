using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WPF.Attributes;
using WPF.Stores;

namespace WPF.ViewModels.Common
{
    /// <summary>
    /// A base model that future custom title bar ViewModels can inherit from
    /// </summary>
    [AssemblyScanIgnore]
    public abstract class TitleBarViewModelBase : ViewModelBase
    {
        public ViewModelBase ContentViewModel { get; }
        private readonly WindowSettingsStore _windowSettingsStore;

        public TitleBarViewModelBase(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore)
        {
            ContentViewModel = contentViewModel;
            _windowSettingsStore = windowSettingsStore;

            (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).Exit += TitleBarViewModelBase_Exit;
        }

        private void TitleBarViewModelBase_Exit(object sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            Debug.WriteLine("Exit event");
            Startup.Stop();
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

        public void MinimizeWindowHandler()
        {
            _windowSettingsStore.CurrentWindowState = WindowState.Minimized;
        }

        public void CloseWindowHandler()
        {
            if(Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifeTime)
            {
                lifeTime.Shutdown();
            }
        }
    }
}
