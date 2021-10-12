using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.Stores.Navigation
{
    public class ModalNavigationStore
    {
        public event Action CurrentModalViewModelChanged;
        private ViewModelBase _currentModalViewModel;
        public ViewModelBase CurrentModalViewModel
        {
            get => _currentModalViewModel;
            set { _currentModalViewModel = value; OnCurrentModalViewModelChanged(); }
        }
        void OnCurrentModalViewModelChanged()
        {
            CurrentModalViewModelChanged?.Invoke();
        }
    }
}
