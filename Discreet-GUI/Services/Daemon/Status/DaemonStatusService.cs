using Services.Daemon.Common;
using Services.Daemon.Status.Responses;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Daemon.Status
{
    /// <summary>
    /// Used to interact with the Status API in the Daemon.
    /// </summary>
    public class DaemonStatusService
    {
        private RPCServer _rpcServer;

        public DaemonStatusService(RPCServer rpcServer)
        {
            _rpcServer = rpcServer;
        }

        /// <summary>
        /// Gets the number of peers connected to the Daemon.
        /// </summary>
        /// <returns>The number of connections the Daemon has, or -1 if the call fails.</returns>
        public async Task<int?> GetNumConnections()
        {
            var req = new DaemonRequest("get_num_connections");
            
            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if(resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<int>((JsonElement)resp.Result);
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Gets the health data of the daemon
        /// </summary>
        /// <returns></returns>
        public async Task<GetHealthResponse> GetHealth()
        {
            var req = new DaemonRequest("get_health");

            var resp = await _rpcServer.Request(req);
            if (resp is null) return null;

            if (resp.ContainsError(out var error))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<GetHealthResponse>((JsonElement)resp.Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
