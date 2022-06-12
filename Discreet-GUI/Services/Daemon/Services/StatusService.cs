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
    /// <summary>
    /// Used to interact with the Status API in the Daemon.
    /// </summary>
    public class StatusService
    {
        private RPCServer _rpcServer;

        public StatusService(RPCServer rpcServer)
        {
            _rpcServer = rpcServer;
        }

        /// <summary>
        /// Gets the number of peers connected to the Daemon.
        /// </summary>
        /// <returns>The number of connections the Daemon has, or -1 if the call fails.</returns>
        public async Task<int> GetNumConnections()
        {
            var req = new DaemonRequest("get_num_connections");
            
            var resp = await _rpcServer.Request(req);

            try
            {
                return JsonSerializer.Deserialize<int>((JsonElement)resp.Result);
            }
            catch
            {
                return -1;
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
