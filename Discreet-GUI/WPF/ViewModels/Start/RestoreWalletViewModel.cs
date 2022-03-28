using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Account;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Layouts.Account;

namespace WPF.ViewModels.Start
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class RestoreWalletViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> NextCommand { get; set; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; set; }
        public ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; set; }

        private string _selectedFilePath = string.Empty;
        public string SelectedFilePath { get => _selectedFilePath; set { _selectedFilePath = value; OnPropertyChanged(nameof(SelectedFilePath)); } }

        public RestoreWalletViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            NextCommand = ReactiveCommand.Create(navigationServiceFactory.CreateAccountNavigation<AccountLeftNavigationLayoutViewModel>().Navigate);
            BackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<ExistingWalletChoicesViewModel>().Navigate);

            OpenFileDialogCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var dialog = new OpenFolderDialog();

                if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var result = await dialog.ShowAsync(desktop.MainWindow);
                    if(result != null)
                    {
                        SelectedFilePath = result;
                    }
                }
            });
        }
    }
}
