using Services.Daemon;
using Services.Daemon.Common;
using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using WPF.Caches;

namespace WPF.Services.Managers
{
    public class WalletManagerService
    {
        private RPCServer _rpcServer;
        private WalletCache _walletCache;

        public WalletManagerService(RPCServer rpcServer, WalletCache walletCache)
        {
            _rpcServer = rpcServer;
            _walletCache = walletCache;
        }

        //public async Task Start(CancellationToken token)
        //{
        //    while (!token.IsCancellationRequested)
        //    {
        //        if (_walletCache.Accounts != null)
        //        {
        //            _walletCache.Accounts.ForEach(async account =>
        //            {
        //                var resp = await _rpcServer.Request(new DaemonRequest("get_balance", account.Address));

        //                if (resp != null)
        //                {
        //                    if (resp.Result.GetType() == typeof(JsonElement))
        //                    {
        //                        var json = (JsonElement)resp.Result;

        //                        if (json.ValueKind == JsonValueKind.Number)
        //                        {
        //                            // the request was good
        //                            account.Balance = json.GetUInt64();
        //                        }
        //                        else
        //                        {
        //                            // error type
        //                            var err = JsonSerializer.Deserialize<DaemonErrorResult>(json);

        //                            System.Diagnostics.Debug.WriteLine("WalletManager getting balance : " + err.ErrMsg);
        //                        }
        //                    }
        //                }
        //            });

        //            await Task.Delay(100);
        //        }
        //    }
        //}
    }
}
