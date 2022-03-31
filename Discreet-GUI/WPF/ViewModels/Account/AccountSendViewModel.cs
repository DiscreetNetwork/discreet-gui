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

        public string Receiver { get; set; }
        public ulong Amount { get; set; }
        public decimal TotalBalance => _walletCache.TotalBalance;

        ObservableCollection<FeeMock> FeeItems { get; set; } = new ObservableCollection<FeeMock>() { new FeeMock { Fee = "12 DIS" }, new FeeMock { Fee = "30 DIS" } };
        int SelectedFeeItemsIndex { get; set; } = 0;


        public ObservableCollectionEx<WalletCache.WalletAddress> SenderAccounts => _walletCache.Accounts;
        int SelectedSenderAccountIndex { get; set; } = 0;


        private RPCServer _rpcServer { get; set; }
        private WalletCache _walletCache { get; set; }

        public ReactiveCommand<Unit, Unit> SendTransaction { get; set; }

        //public AccountSendViewModel() { }
        public AccountSendViewModel(WalletCache walletCache, RPCServer rpcServer, NavigationServiceFactory navigationServiceFactory)
        {
            _walletCache = walletCache;
            _rpcServer = rpcServer;
            _navigationServiceFactory = navigationServiceFactory;
            _walletCache.TotalBalanceChanged += () => OnPropertyChanged(nameof(TotalBalance));


            SendTransaction = ReactiveCommand.CreateFromTask(CreateAndSendTransactionAsync);
        }

        public async Task CreateAndSendTransactionAsync()
        {
            CreateTransactionParam p = new CreateTransactionParam
            {
                Amount = Amount,
                To = Receiver
            };

            CreateTransactionRequest req = new CreateTransactionRequest
            {
                Address = _walletCache.Accounts[0].Address,
                Params = new List<CreateTransactionParam>(new CreateTransactionParam[] { p }),
                Raw = false,
                Relay = true
            };

            await _rpcServer.Request(new DaemonRequest("create_transaction", req));
        }

        public void DisplayConfirm()
        {
            _navigationServiceFactory.CreateModalNavigationService<ConfirmViewModel>().Navigate();
        }

        class FeeMock
        {
            public string Fee { get; set; }
        }
    }
}
