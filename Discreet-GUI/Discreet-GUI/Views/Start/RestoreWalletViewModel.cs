using Services.Caches;
using Services.Daemon;
using System.Threading.Tasks;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.Services;
using Discreet_GUI.Utility;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Account;
using Discreet_GUI.Views.Layouts;
using Discreet_GUI.Views.Layouts.Account;

namespace Discreet_GUI.Views.Start
{
    [Layout(typeof(DarkTitleBarLayoutWithBackButtonViewModel))]
    class RestoreWalletViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly WalletService _walletService;
        private readonly WalletCache _walletCache;
        private readonly NotificationService _notificationService;

        private string _walletName;
        public string WalletName { get => _walletName; set { _walletName = value; ValidateInput(); } }

        // RadioButtons
        private bool _fromMnemonicSeed = true;
        public bool FromMnemonicSeed { get => _fromMnemonicSeed; set { _fromMnemonicSeed = value; OnPropertyChanged(nameof(FromMnemonicSeed)); } }

        private bool _fromKeys = false;
        public bool FromKeys { get => _fromKeys; set { _fromKeys = value; OnPropertyChanged(nameof(FromKeys)); } }

        private string _mnemonicPhrase;
        public string MnemonicPhrase { get => _mnemonicPhrase; set { _mnemonicPhrase = value.Trim(); ValidateInput(); } }


        // Password section
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


        private string _selectedPassword = string.Empty;
        public string SelectedPassword { get => _selectedPassword; set { _selectedPassword = value; PasswordStrength = PasswordStrengthIndicator.CalculatePasswordStrength(value); ValidateInput(); } }

        private string _selectedPasswordConfirm = string.Empty;
        public string SelectedPasswordConfirm { get => _selectedPasswordConfirm; set { _selectedPasswordConfirm = value; ValidateInput(); } }


        // Password strength
        private PasswordStrength _passwordStrength = PasswordStrength.VeryWeak;
        public PasswordStrength PasswordStrength { get => _passwordStrength; set { if (_passwordStrength == value) return; _passwordStrength = value; OnPropertyChanged(nameof(PasswordStrength)); } }

        private bool _canContinue = false;
        public bool CanContinue { get => _canContinue; set { _canContinue = value; OnPropertyChanged(nameof(CanContinue)); } }

        public RestoreWalletViewModel(NavigationServiceFactory navigationServiceFactory, WalletService walletService, WalletCache walletCache, NotificationService notificationService)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _walletService = walletService;
            _walletCache = walletCache;
            _notificationService = notificationService;

            PasswordStrength = PasswordStrengthIndicator.CalculatePasswordStrength(SelectedPassword);
        }

        void ToggleDisplayEnterPasswordCommand()
        {
            DisplayEnterPassword = !DisplayEnterPassword;
            EnterPasswordCharacter = DisplayEnterPassword? string.Empty : "●";
        }

        void ToggleDisplayConfirmPasswordCommand()
        {
            DisplayConfirmPassword = !DisplayConfirmPassword;
            ConfirmPasswordCharacter = DisplayConfirmPassword ? string.Empty : "●";
        }

        void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(WalletName))
            {
                CanContinue = false;
                return;
            }
            if (string.IsNullOrWhiteSpace(MnemonicPhrase))
            {
                CanContinue = false;
                return;
            }
            if (MnemonicPhrase.Split(" ").Length != 24)
            {
                CanContinue = false;
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedPassword))
            {
                CanContinue = false;
                return;
            }
            if (SelectedPassword != SelectedPasswordConfirm)
            {
                CanContinue = false;
                return;
            }

            CanContinue = true;
        }

        async Task NextCommand()
        {
            var wallet = await _walletService.RecoverWallet(WalletName, MnemonicPhrase, SelectedPassword);
            if(wallet is null)
            {
                _notificationService.DisplayInformation("Failed to recover the wallet");
                return;
            }

            _walletCache.Label = wallet.Label;
            _navigationServiceFactory.CreateAccountNavigation<AccountLeftNavigationLayoutViewModel>().Navigate();
            _navigationServiceFactory.CreateAccountNavigation<AccountHomeViewModel>().Navigate();
        }

        void BackCommand()
        {
            _navigationServiceFactory.Create<ExistingWalletChoicesViewModel>().Navigate();
        }
    }
}

