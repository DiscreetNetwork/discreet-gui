using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;
using WPF.ViewModels.Layouts;
using WPF.ViewModels.Start;

namespace WPF.ViewModels.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class WalletPasswordViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateWalletDetailsViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }


        private string _enterPasswordCharacter = "*";
        public string EnterPasswordCharacter { get => _enterPasswordCharacter; set { _enterPasswordCharacter = value; OnPropertyChanged(nameof(EnterPasswordCharacter)); } }

        private bool _displayEnterPassword = false;
        public bool DisplayEnterPassword { get => _displayEnterPassword; set { _displayEnterPassword = value; OnPropertyChanged(nameof(DisplayEnterPassword)); } }
        ReactiveCommand<Unit, Unit> ToggleDisplayEnterPasswordCommand { get; set; }


        private string _confirmPasswordCharacter = "*";
        public string ConfirmPasswordCharacter { get => _confirmPasswordCharacter; set { _confirmPasswordCharacter = value; OnPropertyChanged(nameof(ConfirmPasswordCharacter)); } }

        private bool _displayConfirmPassword = false;

        public bool DisplayConfirmPassword { get => _displayConfirmPassword; set { _displayConfirmPassword = value; OnPropertyChanged(nameof(DisplayConfirmPassword)); } }
        ReactiveCommand<Unit, Unit> ToggleDisplayConfirmPasswordCommand { get; set; }


        private readonly NewWalletCache _newWalletCache;

        public string SelectedPassword { get => _newWalletCache.Password; set => _newWalletCache.Password = value; }

        private string _selectedPasswordConfirm = string.Empty;
        public string SelectedPasswordConfirm { get => _selectedPasswordConfirm; set { _selectedPasswordConfirm = value; } }

        public WalletPasswordViewModel() { }

        public WalletPasswordViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache)
        {
            NavigateWalletDetailsViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletDetailsViewModel>().Navigate);
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<VerifyRecoveryPhraseViewModel>().Navigate);

            ToggleDisplayEnterPasswordCommand = ReactiveCommand.Create(() =>
            {
                DisplayEnterPassword = !DisplayEnterPassword;
                EnterPasswordCharacter = DisplayEnterPassword ? string.Empty : "*";
            });

            ToggleDisplayConfirmPasswordCommand = ReactiveCommand.Create(() =>
            {
                DisplayConfirmPassword = !DisplayConfirmPassword;
                ConfirmPasswordCharacter = DisplayConfirmPassword ? string.Empty : "*";
            });
            
            _newWalletCache = newWalletCache;
        }
    }
}
