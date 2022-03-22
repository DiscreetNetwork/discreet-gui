using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;

namespace WPF.ViewModels.Modals
{
    [Layout(typeof(DarkTitleBarLayoutSimpleViewModel))]
    public class PasswordViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> EnterCommand { get; set; }


        private bool _displayPassword = false;
        public bool DisplayPassword { get => _displayPassword; set { _displayPassword = value; OnPropertyChanged(nameof(DisplayPassword)); } }
        private string _passwordCharacter = "*";
        public string PasswordCharacter { get => _passwordCharacter; set { _passwordCharacter = value; OnPropertyChanged(nameof(PasswordCharacter)); } }
        ReactiveCommand<Unit, Unit> ToggleDisplayPasswordCommand { get; set; }

        public PasswordViewModel() { }

        public PasswordViewModel(NavigationServiceFactory navigationServiceFactory)
        {
            EnterCommand = ReactiveCommand.Create(navigationServiceFactory.CreateModalNavigationService().Navigate);
            ToggleDisplayPasswordCommand = ReactiveCommand.Create(() =>
            {
                DisplayPassword = !DisplayPassword;
                PasswordCharacter = DisplayPassword ? string.Empty : "*";
            });
        }
    }
}
