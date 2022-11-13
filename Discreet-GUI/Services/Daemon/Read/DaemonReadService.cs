using Services.Daemon.Common;
using Services.Daemon.Read.Requests;
using System.Threading.Tasks;

namespace Services.Daemon.Read
{
    public class DaemonReadService
    {
        private readonly RPCServer _rpcServer;

        public DaemonReadService(RPCServer rpcServer)
        {
            _rpcServer = rpcServer;
        }


        public async Task<bool> VerifyAddress(VerifyAddressRequest @params)
        {
            var req = new DaemonRequest("verify_address", @params.Address);

            var resp = await _rpcServer.Request(req);
            if (resp is null) return false;

            if (resp.ContainsError(out var error))
            {
                return false;
            }

            return true;
        }
    }
}
