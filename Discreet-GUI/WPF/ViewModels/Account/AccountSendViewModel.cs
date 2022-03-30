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
using WPF.ViewModels.Common;
using WPF.Views.Layouts;

namespace WPF.ViewModels.Account
{
    public class AccountSendViewModel : ViewModelBase
    {
        public string Receiver { get; set; }
        public ulong Amount { get; set; }

        ObservableCollection<FeeMock> FeeItems { get; set; } = new ObservableCollection<FeeMock>() { new FeeMock { Fee = "12 DIS" }, new FeeMock { Fee = "30 DIS" } };
        int SelectedFeeItemsIndex { get; set; } = 0;
        ObservableCollection<SenderAccountMock> SenderAccountItems { get; set; } = new ObservableCollection<SenderAccountMock>() { new SenderAccountMock { Address = "123asd456xtg" }, new SenderAccountMock { Address = "85321sajcv932" } };
        int SelectedSenderAccountItemsIndex { get; set; } = 0;

        private RPCServer _rpcServer { get; set; }
        private WalletCache _walletCache { get; set; }

        public ReactiveCommand<Unit, Unit> SendTransaction { get; set; }

        public AccountSendViewModel() { }
        public AccountSendViewModel(WalletCache walletCache, RPCServer rpcServer)
        {
            _walletCache = walletCache;
            _rpcServer = rpcServer;

            SendTransaction = ReactiveCommand.Create(CreateAndSendTransaction);

            SendTransaction = ReactiveCommand.CreateFromTask(CreateAndSendTransactionAsync); // New
        }

        public void CreateAndSendTransaction()
        {
            _ = Task.Run(async () =>
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
            }).ConfigureAwait(false);
            
        }

        // New
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



        class FeeMock
        {
            public string Fee { get; set; }
        }

        class SenderAccountMock
        {
            public string Address { get; set; }
        }
    }
}
