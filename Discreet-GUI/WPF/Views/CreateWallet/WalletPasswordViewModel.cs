using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Services.Caches;
using WPF.Factories.Navigation;
using WPF.Utility;
using WPF.ViewModels.Common;
using WPF.Views.Layouts;
using WPF.Views.Start;

namespace WPF.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class WalletPasswordViewModel : ViewModelBase
    {
        ReactiveCommand<Unit, Unit> NavigateWalletDetailsViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public string EnterPasswordFontSize { get => DisplayEnterPassword ? "14" : "10"; }

        private string _enterPasswordCharacter = "●";
        public string EnterPasswordCharacter { get => _enterPasswordCharacter; set { _enterPasswordCharacter = value; OnPropertyChanged(nameof(EnterPasswordCharacter)); } }

        private bool _displayEnterPassword = false;
        public bool DisplayEnterPassword { get => _displayEnterPassword; set { _displayEnterPassword = value; OnPropertyChanged(nameof(DisplayEnterPassword)); OnPropertyChanged(nameof(EnterPasswordFontSize)); } }
        ReactiveCommand<Unit, Unit> ToggleDisplayEnterPasswordCommand { get; set; }


        public string ConfirmPasswordFontSize { get => DisplayConfirmPassword ? "14" : "10"; }

        private string _confirmPasswordCharacter = "●";
        public string ConfirmPasswordCharacter { get => _confirmPasswordCharacter; set { _confirmPasswordCharacter = value; OnPropertyChanged(nameof(ConfirmPasswordCharacter)); } }

        private bool _displayConfirmPassword = false;

        public bool DisplayConfirmPassword { get => _displayConfirmPassword; set { _displayConfirmPassword = value; OnPropertyChanged(nameof(DisplayConfirmPassword)); OnPropertyChanged(nameof(ConfirmPasswordFontSize)); } }
        ReactiveCommand<Unit, Unit> ToggleDisplayConfirmPasswordCommand { get; set; }


        private readonly NewWalletCache _newWalletCache;

        public string SelectedPassword { get => _newWalletCache.Password; set { _newWalletCache.Password = value; PasswordStrength = PasswordStrengthIndicator.CalculatePasswordStrength(value); ValidateCanContinue(); } }

        private string _selectedPasswordConfirm = string.Empty;
        public string SelectedPasswordConfirm { get => _selectedPasswordConfirm; set { _selectedPasswordConfirm = value; ValidateCanContinue(); } }


        // Password strength
        private PasswordStrength _passwordStrength = PasswordStrength.VeryWeak;
        public PasswordStrength PasswordStrength { get => _passwordStrength; set { if (_passwordStrength == value) return; _passwordStrength = value; OnPropertyChanged(nameof(PasswordStrength)); } }




        private bool _canContinue = false;
        public bool CanContinue { get => _canContinue; set { _canContinue = value; OnPropertyChanged(nameof(CanContinue)); } }

        public WalletPasswordViewModel() { }

        public WalletPasswordViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache)
        {
            NavigateWalletDetailsViewCommand = ReactiveCommand.Create(navigationServiceFactory.Create<WalletDetailsViewModel>().Navigate);
            NavigateBackCommand = ReactiveCommand.Create(navigationServiceFactory.Create<VerifyRecoveryPhraseViewModel>().Navigate);

            ToggleDisplayEnterPasswordCommand = ReactiveCommand.Create(() =>
            {
                DisplayEnterPassword = !DisplayEnterPassword;
                EnterPasswordCharacter = DisplayEnterPassword ? string.Empty : "●";
            });

            ToggleDisplayConfirmPasswordCommand = ReactiveCommand.Create(() =>
            {
                DisplayConfirmPassword = !DisplayConfirmPassword;
                ConfirmPasswordCharacter = DisplayConfirmPassword ? string.Empty : "●";
            });
            
            _newWalletCache = newWalletCache;

            PasswordStrength = PasswordStrengthIndicator.CalculatePasswordStrength(SelectedPassword);
            ValidateCanContinue();
        }

        public void ValidateCanContinue()
        {
            if(string.IsNullOrWhiteSpace(SelectedPassword))
            {
                CanContinue = false;
                return;
            }

            if(string.IsNullOrWhiteSpace(SelectedPasswordConfirm))
            {
                CanContinue = false;
                return;
            }

            if(!SelectedPassword.Equals(SelectedPasswordConfirm))
            {
                CanContinue = false;
                return;
            }

            CanContinue = true;
        }
    }
}
