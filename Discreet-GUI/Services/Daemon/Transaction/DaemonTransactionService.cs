using Services.Daemon.Common;
using Services.Daemon.Transaction.Requests;
using Services.Daemon.Transaction.Responses;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Daemon.Transaction
{
    public class DaemonTransactionService
    {
        private readonly RPCServer _rpcServer;

        public DaemonTransactionService(RPCServer rpcServer)
        {
            _rpcServer = rpcServer;
        }

        public async Task<CreateTransactionResponse> CreateTransaction(string sender, string receiver, ulong amount)
        {
            CreateTransactionParam p = new CreateTransactionParam
            {
                Amount = amount,
                To = receiver
            };

            CreateTransactionRequest txRequest = new CreateTransactionRequest
            {
                Address = sender,
                Params = new List<CreateTransactionParam>(new CreateTransactionParam[] { p }),
                Raw = false,
                Relay = true
            };

            var req = new DaemonRequest("create_transaction", txRequest);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                var createTransactionResponse = JsonSerializer.Deserialize<CreateTransactionResponse>((JsonElement)resp.Result);
                return string.IsNullOrWhiteSpace(createTransactionResponse.Error) ? createTransactionResponse : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
