using System;
using System.Collections.Generic;
using System.Text;
using WPF.Factories.ViewModel;
using WPF.ViewModels.Common;
using WPF.Views.Settings.Views;

namespace WPF.Views.Settings
{
    class SettingsViewModel : ViewModelBase
    {
        private ViewModelBase _selectedSettingsViewModel = null;
        private readonly LayoutViewModelFactory _layoutViewModelFactory;

        public ViewModelBase SelectedSettingsViewModel { get => _selectedSettingsViewModel; set { _selectedSettingsViewModel = value; OnPropertyChanged(nameof(SelectedSettingsViewModel)); } }

        public SettingsViewModel(LayoutViewModelFactory layoutViewModelFactory)
        {
            _layoutViewModelFactory = layoutViewModelFactory;
        }

        public void DisplayLogWindow()
        {
            SelectedSettingsViewModel = _layoutViewModelFactory.Create<LogViewModel>();
        }
    }
}
