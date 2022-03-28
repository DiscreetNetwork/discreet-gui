using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.Views;

namespace WPF.ViewModels.Start
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class OpenWalletFromFileViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> OpenFileDialogCommand { get; set; }

        public OpenWalletFromFileViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            OpenFileDialogCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var dialog = new OpenFileDialog();
                dialog.Filters.Add(new FileDialogFilter { Name = "All", Extensions = { "*" } });
                
                if(Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var result = await dialog.ShowAsync(desktop.MainWindow);
                }
            });
        }
    }
}
