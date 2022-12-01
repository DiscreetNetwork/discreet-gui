using ReactiveUI;
using System.Reactive;
using Services.Caches;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Utility;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Layouts;

namespace Discreet_GUI.Views.CreateWallet
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    public class WalletPasswordViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly NewWalletCache _newWalletCache;

        public string EnterPasswordFontSize { get => DisplayEnterPassword ? "14" : "10"; }

        private string _enterPasswordCharacter = "●";
        public string EnterPasswordCharacter { get => _enterPasswordCharacter; set { _enterPasswordCharacter = value; OnPropertyChanged(nameof(EnterPasswordCharacter)); } }

        private bool _displayEnterPassword = false;
        public bool DisplayEnterPassword { get => _displayEnterPassword; set { _displayEnterPassword = value; OnPropertyChanged(nameof(DisplayEnterPassword)); OnPropertyChanged(nameof(EnterPasswordFontSize)); } }


        public string ConfirmPasswordFontSize { get => DisplayConfirmPassword ? "14" : "10"; }

        private string _confirmPasswordCharacter = "●";
        public string ConfirmPasswordCharacter { get => _confirmPasswordCharacter; set { _confirmPasswordCharacter = value; OnPropertyChanged(nameof(ConfirmPasswordCharacter)); } }


        private bool _displayConfirmPassword = false;
        public bool DisplayConfirmPassword { get => _displayConfirmPassword; set { _displayConfirmPassword = value; OnPropertyChanged(nameof(DisplayConfirmPassword)); OnPropertyChanged(nameof(ConfirmPasswordFontSize)); } }

        
        public string SelectedPassword { get => _newWalletCache.Password; set { _newWalletCache.Password = value; PasswordStrength = PasswordStrengthIndicator.CalculatePasswordStrength(value); ValidateCanContinue(); } }
        private string _selectedPasswordConfirm = string.Empty;
        public string SelectedPasswordConfirm { get => _selectedPasswordConfirm; set { _selectedPasswordConfirm = value; ValidateCanContinue(); } }


        // Password strength
        private PasswordStrength _passwordStrength = PasswordStrength.VeryWeak;
        public PasswordStrength PasswordStrength { get => _passwordStrength; set { if (_passwordStrength == value) return; _passwordStrength = value; OnPropertyChanged(nameof(PasswordStrength)); } }




        private bool _canContinue = false;
        public bool CanContinue { get => _canContinue; set { _canContinue = value; OnPropertyChanged(nameof(CanContinue)); } }

        public WalletPasswordViewModel(NavigationServiceFactory navigationServiceFactory, NewWalletCache newWalletCache)
        {
            _navigationServiceFactory = navigationServiceFactory;
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

        void NavigateWalletDetailsViewCommand()
        {
            _navigationServiceFactory.Create<WalletDetailsViewModel>().Navigate();
        }

        void NavigateBackCommand()
        {
            _navigationServiceFactory.Create<VerifyRecoveryPhraseViewModel>().Navigate();
        }

        void ToggleDisplayEnterPasswordCommand()
        {
            DisplayEnterPassword = !DisplayEnterPassword;
            EnterPasswordCharacter = DisplayEnterPassword ? string.Empty : "●";
        }

        void ToggleDisplayConfirmPasswordCommand()
        {
            DisplayConfirmPassword = !DisplayConfirmPassword;
            ConfirmPasswordCharacter = DisplayConfirmPassword ? string.Empty : "●";
        }
    }
}
