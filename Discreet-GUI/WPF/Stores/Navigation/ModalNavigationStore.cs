using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels.Common;

namespace WPF.Stores.Navigation
{
    /// <summary>
    /// A store to hold the current Modal ViewModel, used to display notifications, popups etc.
    /// </summary>
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
