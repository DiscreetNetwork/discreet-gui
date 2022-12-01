using System;
using Discreet_GUI.ViewModels.Common;

namespace Discreet_GUI.Stores.Navigation
{
    public class SettingsNavigationStore
    {
        public event Action CurrentViewModelChanged;
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnCurrentViewModelChanged(); }
        }
        void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
