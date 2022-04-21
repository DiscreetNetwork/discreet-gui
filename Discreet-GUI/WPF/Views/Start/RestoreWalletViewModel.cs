using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.Views.Account;
using WPF.Views.Layouts;
using WPF.Views.Layouts.Account;

namespace WPF.Views.Start
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class RestoreWalletViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;

        public string WalletName { get; set; }

        private string _selectedFilePath = string.Empty;
        public string SelectedFilePath { get => _selectedFilePath; set { _selectedFilePath = value; OnPropertyChanged(nameof(SelectedFilePath)); } }

        // RadioButtons
        private bool _fromMnemonicSeed = true;
        public bool FromMnemonicSeed { get => _fromMnemonicSeed; set { _fromMnemonicSeed = value; OnPropertyChanged(nameof(FromMnemonicSeed)); } }

        private bool _fromKeys = false;
        public bool FromKeys { get => _fromKeys; set { _fromKeys = value; OnPropertyChanged(nameof(FromKeys)); } }

        public string MnemonicPhrase { get; set; }

        public RestoreWalletViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            _navigationServiceFactory = navigationServiceFactory;
        }

        void NextCommand()
        {
            _navigationServiceFactory.CreateAccountNavigation<AccountLeftNavigationLayoutViewModel>().Navigate();
            _navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
        }

        void BackCommand()
        {
            _navigationServiceFactory.Create<ExistingWalletChoicesViewModel>().Navigate();
        }

        async Task OpenFileDialogCommand()
        {
            var dialog = new OpenFolderDialog();

            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var result = await dialog.ShowAsync(desktop.MainWindow);
                if (result != null)
                {
                    SelectedFilePath = result;
                }
            }
        }
    }
}
