using Discreet_GUI.Factories.ViewModel;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Settings.Views;

namespace Discreet_GUI.Views.Settings
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
