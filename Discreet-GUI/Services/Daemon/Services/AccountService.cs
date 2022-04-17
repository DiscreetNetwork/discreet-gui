using Services.Daemon.Common;
using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Daemon.Services
{
    public class AccountService
    {
        private readonly RPCServer _rpcServer;

        public AccountService(RPCServer rpcServer)
        {
            _rpcServer = rpcServer;
        }

        public async Task<ulong?> GetBalance(string accountAddress)
        {
            var req = new DaemonRequest("get_balance", accountAddress);

            var resp = await _rpcServer.Request(req);

            if (resp != null && resp.Result != null)
            {
                if (resp.Result is JsonElement json)
                {
                    if (json.ValueKind == JsonValueKind.Number)
                    {
                        // the request was good
                        var balance = json.GetUInt64();
                        return balance;
                    }
                    else
                    {
                        // error type
                        var err = JsonSerializer.Deserialize<DaemonErrorResult>(json);

                        System.Diagnostics.Debug.WriteLine("WalletManager getting balance : " + err.ErrMsg);
                    }
                }
            }

            return null;
        }

        public async Task<GetAddressHeightResponse> GetState(string accountAddress)
        {
            var req = new DaemonRequest("get_address_height", accountAddress);

            var resp = await _rpcServer.Request(req);

            if (resp != null && resp.Result != null)
            {
                if (resp.Result is JsonElement json)
                {
                    var getAddressHeightResponse = JsonSerializer.Deserialize<GetAddressHeightResponse>(json);

                    if (getAddressHeightResponse.ErrMsg != null && getAddressHeightResponse.ErrMsg != "")
                    {
                        System.Diagnostics.Debug.WriteLine("WalletManager getting address height : " + getAddressHeightResponse.ErrMsg);
                    }

                    return getAddressHeightResponse;
                }
            }

            return null;
        }
    }
}
