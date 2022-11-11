using Avalonia.Controls;
using System;

namespace Discreet_GUI.Stores
{
    public class WindowSettingsStore
    {
        public event Action CurrentWindowStateChanged;
        private WindowState _currentWindowState = WindowState.Normal;
        public WindowState CurrentWindowState
        {
            get => _currentWindowState;
            set { _currentWindowState = value; OnCurrentWindowStateChanged(); }
        }
        public void OnCurrentWindowStateChanged()
        {
            CurrentWindowStateChanged?.Invoke();
        }
    }
}
