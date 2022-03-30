using ReactiveUI;
using Services.Daemon;
using Services.Daemon.Common;
using Services.Daemon.Requests;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.ViewModels.Common;
using WPF.Views.Layouts;

namespace WPF.ViewModels.Account
{
    class AccountSendViewModel : ViewModelBase
    {
        public string Receiver { get; set; }
        public ulong Amount { get; set; }

        private RPCServer _rpcServer { get; set; }
        private WalletCache _walletCache { get; set; }

        public ReactiveCommand<Unit, Unit> SendTransaction { get; set; }

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
    }
}
