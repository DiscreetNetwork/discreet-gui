using Services.Caches;
using Services.Extensions;
using System.Collections.ObjectModel;
using Discreet_GUI.Factories.Navigation;
using Discreet_GUI.ViewModels.Common;
using Discreet_GUI.Views.Modals;
using Services;
using Services.Daemon.Read;
using System.Threading.Tasks;
using Services.Daemon.Read.Requests;

namespace Discreet_GUI.Views.Account
{
    public class AccountSendViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly SendTransactionCache _sendTransactionCache;
        private readonly DaemonReadService _daemonReadService;
        private readonly WalletCache _walletCache;

        string _receiver;
        public string Receiver { get => _receiver; set { _receiver = value; _ = ValidateReceiverInput(); } }
        decimal _amount;
        public decimal Amount { get => _amount; set { _amount = value; ValidateAmountInput(); } }


        // Validation error messages
        string _receiverValidationMessage;
        public string ReceiverValidationMessage { get => _receiverValidationMessage; set { _receiverValidationMessage = value; OnPropertyChanged(nameof(ReceiverValidationMessage)); } }

        string _amountValidationMessage;
        public string AmountValidationMessage { get => _amountValidationMessage; set { _amountValidationMessage = value; OnPropertyChanged(nameof(AmountValidationMessage)); } }


        ObservableCollection<FeeMock> FeeItems { get; set; } = new ObservableCollection<FeeMock>() { new FeeMock { Fee = "Low" }, new FeeMock { Fee = "Average" }, new FeeMock { Fee = "Priority" } };
        int SelectedFeeItemsIndex { get; set; } = 0;


        public ObservableCollectionEx<WalletCache.WalletAddress> SenderAccounts => _walletCache.Accounts; 
        public WalletCache.WalletAddress SelectedAccount => SenderAccounts[SelectedSenderAccountIndex]; 


        int _selectedSenderAccountIndex = 0;
        int SelectedSenderAccountIndex
        {
            get => _selectedSenderAccountIndex;
            set
            {
                // When the ComboBox rerenders, value will be -1, and it will end up selecting the first account again
                // Currently we ignore that, because if the second+ account is selected, and we receive DIS on any account, it will rerender and display the first account again
                // We always want to stay at the selected account. This "fix" might cause issues later, when we allow deleting accounts etc.
                // Possible future fix, is to also check that `SelectedSenderAccountIndex` is not greater than the amount of accounts, in that case, we would allow the index to reset back to 0
                if (value < 0) return;
                else _selectedSenderAccountIndex = value; 
                OnPropertyChanged(nameof(SelectedAccount));
                ValidateAmountInput();
            }
        }

        public AccountSendViewModel(WalletCache walletCache, NavigationServiceFactory navigationServiceFactory, SendTransactionCache sendTransactionCache, DaemonReadService daemonReadService)
        {
            _walletCache = walletCache;
            _navigationServiceFactory = navigationServiceFactory;
            _sendTransactionCache = sendTransactionCache;
            _daemonReadService = daemonReadService;
        }

        async Task<bool> ValidateReceiverInput()
        {
            if (string.IsNullOrWhiteSpace(Receiver))
            {
                ReceiverValidationMessage = "Receiver address cannot be empty";
                return false;
            }

            if(!await _daemonReadService.VerifyAddress(new VerifyAddressRequest(Receiver)))
            {
                ReceiverValidationMessage = "Receiver address is not a valid stealth or transparent address";
                return false;
            }

            ReceiverValidationMessage = string.Empty;
            return true;
        }

        bool ValidateAmountInput()
        {
            if(Amount == 0)
            {
                AmountValidationMessage = "Amount must be greater than 0";
                return false;
            }

            ulong? amountMultiplied = DISTConverter.Multiply(Amount);
            if(amountMultiplied is null)
            {
                AmountValidationMessage = "NaN";
                return false;
            }

            if (amountMultiplied.Value > SelectedAccount.Balance)
            {
                AmountValidationMessage = "Not enough DIST in the selected account";
                return false;
            }

            AmountValidationMessage = string.Empty;
            return true;
        }
        
        public async Task DisplayConfirm()
        {
            if (!await ValidateReceiverInput() || !ValidateAmountInput()) return;

            _sendTransactionCache.Amount = Amount;
            _sendTransactionCache.Receiver = Receiver;
            _sendTransactionCache.Sender = SenderAccounts[SelectedSenderAccountIndex].Address;

            if(SenderAccounts[_selectedSenderAccountIndex].Type == WalletCache.AddressType.TRANSPARENT)
            {
                _navigationServiceFactory.CreateModalNavigationService<TransparentTransactionWarningViewModel>().Navigate();
            }
            else
            {
                _navigationServiceFactory.CreateModalNavigationService<ConfirmViewModel>().Navigate();
            }
        }


        class FeeMock
        {
            public string Fee { get; set; }
        }
    }
}
