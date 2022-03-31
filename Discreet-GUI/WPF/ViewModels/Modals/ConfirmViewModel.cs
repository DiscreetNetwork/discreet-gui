using Services.Daemon;
using Services.Daemon.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Caches;
using WPF.Factories.Navigation;
using WPF.ViewModels.Common;

namespace WPF.ViewModels.Modals
{
    public class ConfirmViewModel : ViewModelBase
    {
        private readonly NavigationServiceFactory _navigationServiceFactory;
        private readonly RPCServer _rpcServer;
        private readonly WalletCache _walletCache;
        private readonly SendTransactionCache _sendTransactionCache;

        public ConfirmViewModel(NavigationServiceFactory navigationServiceFactory, RPCServer rpcServer, WalletCache walletCache, SendTransactionCache sendTransactionCache)
        {
            _navigationServiceFactory = navigationServiceFactory;
            _rpcServer = rpcServer;
            _walletCache = walletCache;
            _sendTransactionCache = sendTransactionCache;
        }

        public void CancelTransactionCommand()
        {
            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }

        public void ConfirmTransactionCommand()
        {
            CreateTransactionParam p = new CreateTransactionParam
            {
                Amount = _sendTransactionCache.Amount,
                To = _sendTransactionCache.Receiver
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
