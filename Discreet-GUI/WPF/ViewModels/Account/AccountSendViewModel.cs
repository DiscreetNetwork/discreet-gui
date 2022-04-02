using Avalonia.Controls.Notifications;
using ReactiveUI;
using Services.Daemon;
using Services.Daemon.Common;
using Services.Daemon.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.ExtensionMethods;
using WPF.Factories.Navigation;
using WPF.Services;
using WPF.ViewModels.Common;
using WPF.ViewModels.Modals;
using WPF.Views.Layouts;

namespace WPF.ViewModels.Account
{
    public class AccountSendViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly SendTransactionCache _sendTransactionCache;
        private readonly WalletCache _walletCache;


        string _receiver;
        public string Receiver { get => _receiver; set { _receiver = value; ValidateReceiverInput(); } }
        double _amount;
        public double Amount { get => _amount; set { _amount = value; ValidateAmountInput(); } }


        // Validation error messages
        string _receiverValidationMessage;
        public string ReceiverValidationMessage { get => _receiverValidationMessage; set { _receiverValidationMessage = value; OnPropertyChanged(nameof(ReceiverValidationMessage)); } }

        string _amountValidationMessage;
        public string AmountValidationMessage { get => _amountValidationMessage; set { _amountValidationMessage = value; OnPropertyChanged(nameof(AmountValidationMessage)); } }


        ObservableCollection<FeeMock> FeeItems { get; set; } = new ObservableCollection<FeeMock>() { new FeeMock { Fee = "12 DIS" }, new FeeMock { Fee = "30 DIS" } };
        int SelectedFeeItemsIndex { get; set; } = 0;


        public ObservableCollectionEx<WalletCache.WalletAddress> SenderAccounts => _walletCache.Accounts; 
        public WalletCache.WalletAddress SelectedAccount => SenderAccounts[SelectedSenderAccountIndex]; 


        int _selectedSenderAccountIndex = 0;
        int SelectedSenderAccountIndex
        {
            get => _selectedSenderAccountIndex;
            set
            {
                if (value < 0) _selectedSenderAccountIndex = 0;
                else _selectedSenderAccountIndex = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        public AccountSendViewModel(WalletCache walletCache, NavigationServiceFactory navigationServiceFactory, SendTransactionCache sendTransactionCache)
        {
            _walletCache = walletCache;
            _navigationServiceFactory = navigationServiceFactory;
            _sendTransactionCache = sendTransactionCache;
        }

        bool ValidateReceiverInput()
        {
            if (string.IsNullOrWhiteSpace(Receiver))
            {
                ReceiverValidationMessage = "Receiver address cannot be empty";
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

            if(Amount > SelectedAccount.Balance)
            {
                AmountValidationMessage = "Not enough DIST in the selected account";
                return false;
            }

            AmountValidationMessage = string.Empty;
            return true;
        }

        public void DisplayConfirm()
        {
            if (!ValidateReceiverInput() || !ValidateAmountInput()) return;

            _sendTransactionCache.Amount = Amount;
            _sendTransactionCache.Receiver = Receiver;
            _sendTransactionCache.Sender = SenderAccounts[SelectedSenderAccountIndex].Address;

            _navigationServiceFactory.CreateModalNavigationService<ConfirmViewModel>().Navigate();
        }


        class FeeMock
        {
            public string Fee { get; set; }
        }
    }
}
