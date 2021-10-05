using System;
using System.Collections.Generic;
using System.Text;
using WPF.Stores.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainNavigationStore _mainNavigationStore;

        public ViewModelBase CurrentViewModel => _mainNavigationStore.CurrentViewModel;

        public MainWindowViewModel(MainNavigationStore mainNavigationStore)
        {
            _mainNavigationStore = mainNavigationStore;
            _mainNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
