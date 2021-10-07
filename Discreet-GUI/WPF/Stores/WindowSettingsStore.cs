using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPF.Stores
{
    public class WindowSettingsStore
    {
        public event Action CurrentWindowStateChanged;
        private WindowState _currentWindowState = WindowState.Normal;
        public WindowState CurrentWindowState
        {
            get => _currentWindowState;
            private set { _currentWindowState = value; OnCurrentWindowStateChanged(); }
        }
        public void OnCurrentWindowStateChanged()
        {
            CurrentWindowStateChanged?.Invoke();
        }
    }
}
