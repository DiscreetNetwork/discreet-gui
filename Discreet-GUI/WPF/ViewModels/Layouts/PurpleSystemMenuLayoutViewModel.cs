using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Stores;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Layouts
{
    public class PurpleSystemMenuLayoutViewModel : ViewModelBase
    {
        private readonly WindowSettingsStore _windowSettingsStore;
        public ReactiveCommand<Unit, Unit> ToggleWindowStateCommand { get; }
        public ViewModelBase ContentViewModel { get; }

        public PurpleSystemMenuLayoutViewModel(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore)
        {
            ContentViewModel = contentViewModel;
            _windowSettingsStore = windowSettingsStore;

            ToggleWindowStateCommand = ReactiveCommand.Create(() =>
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
            });
        }
    }
}
