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


        public string Receiver { get; set; }
        public double Amount { get; set; }


        decimal _selectedAccountBalance;
        public decimal SelectedAccountBalance { get => _selectedAccountBalance; set { _selectedAccountBalance = value; OnPropertyChanged(nameof(SelectedAccountBalance)); } }

        
        ObservableCollection<FeeMock> FeeItems { get; set; } = new ObservableCollection<FeeMock>() { new FeeMock { Fee = "12 DIS" }, new FeeMock { Fee = "30 DIS" } };
        int SelectedFeeItemsIndex { get; set; } = 0;


        public ObservableCollectionEx<WalletCache.WalletAddress> SenderAccounts { get => _walletCache.Accounts; set => SenderAccountsChanged(); }
        public WalletCache.WalletAddress SelectedAccount => SenderAccounts[SelectedSenderAccountIndex]; 


        int _selectedSenderAccountIndex = 0;
        int SelectedSenderAccountIndex { get => _selectedSenderAccountIndex; set { _selectedSenderAccountIndex = value; OnPropertyChanged(nameof(SelectedAccount)); } }


        public AccountSendViewModel(WalletCache walletCache, NavigationServiceFactory navigationServiceFactory, SendTransactionCache sendTransactionCache)
        {
            _walletCache = walletCache;
            _navigationServiceFactory = navigationServiceFactory;
            _sendTransactionCache = sendTransactionCache;
        }

        public void SenderAccountsChanged()
        {
            //SelectedAccountBalance = SenderAccounts[SelectedSenderAccountIndex].Balance;
        }

        public void DisplayConfirm()
        {
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
