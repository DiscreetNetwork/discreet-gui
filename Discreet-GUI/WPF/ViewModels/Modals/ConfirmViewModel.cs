using Services.Daemon;
using Services.Daemon.Common;
using Services.Daemon.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        public async Task ConfirmTransactionCommand()
        {
            CreateTransactionParam p = new CreateTransactionParam
            {
                Amount = (ulong)_sendTransactionCache.Amount,
                To = _sendTransactionCache.Receiver
            };

            CreateTransactionRequest req = new CreateTransactionRequest
            {
                Address = _sendTransactionCache.Sender,
                Params = new List<CreateTransactionParam>(new CreateTransactionParam[] { p }),
                Raw = false,
                Relay = true
            };


            DaemonResponse resp = null;
            try
            {
                resp = await _rpcServer.Request(new DaemonRequest("create_transaction", req));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _navigationServiceFactory.CreateModalNavigationService().Navigate();
            }

            if (resp != null && resp.Result != null)
            {
                if (resp.Result is JsonElement json) 
                {
                    try
                    {
                        DaemonErrorResult errResult = JsonSerializer.Deserialize<DaemonErrorResult>(json);
                        System.Diagnostics.Debug.WriteLine($"CreateTransactionResponse: {errResult.ErrMsg}");
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine($"CreateTransactionResponse: {e.Message}");
                    }
                } 
            }

            _navigationServiceFactory.CreateModalNavigationService().Navigate();
        }
    }
}
