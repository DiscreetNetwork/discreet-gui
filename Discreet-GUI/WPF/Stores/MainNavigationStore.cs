using System;
using System.Collections.Generic;
using System.Text;
using WPF.ViewModels;

namespace WPF.Stores
{
    /// <summary>
    /// A Store to hold the ViewModelBase, that the MainWindow.xaml will be displaying
    /// </summary>
    public class MainNavigationStore
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
