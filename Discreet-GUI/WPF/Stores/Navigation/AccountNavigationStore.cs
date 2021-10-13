using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.Stores.Navigation
{
    public class AccountNavigationStore
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
