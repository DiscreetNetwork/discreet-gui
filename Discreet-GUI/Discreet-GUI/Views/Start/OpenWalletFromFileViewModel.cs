using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views;
using Discreet_GUI.Views.Layouts;

namespace Discreet_GUI.Views.Start
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
