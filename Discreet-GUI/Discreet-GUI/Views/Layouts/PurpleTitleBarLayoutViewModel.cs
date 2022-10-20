using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Text;
using System.Windows.Input;
using Discreet_GUI.Stores;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Views.Layouts
{
    public class PurpleTitleBarLayoutViewModel : TitleBarViewModelBase
    {
        public PurpleTitleBarLayoutViewModel(ViewModelBase contentViewModel, WindowSettingsStore windowSettingsStore) : base(contentViewModel, windowSettingsStore)
        {
        }
    }
}
