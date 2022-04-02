using Services.Daemon.Common;
using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace Services.Daemon
{
    public class RPCServer
    {
        private WebClient _client;

        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public RPCServer()
        {
            _client = new WebClient();
            _client.BaseAddress = "http://localhost:8350";
        }

        /// <summary>
        /// Sends a request asynchronously.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<DaemonResponse> Request(DaemonRequest req)
        {
            await _semaphore.WaitAsync();

            var responseText = _client.UploadString("/", req.Serialize());

            if(string.IsNullOrWhiteSpace(responseText))
            {
                _semaphore.Release();

                return null;
            }

            var resp = DaemonResponse.Deserialize(responseText);

            _semaphore.Release();

            return resp;
        }

        /// <summary>
        /// Sends a synchronous request. Only use if requests are known to be done in order.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DaemonResponse RequestUnsafe(DaemonRequest req)
        {
            var responseText = _client.UploadString("/", req.Serialize());
            var resp = DaemonResponse.Deserialize(responseText);

            return resp;
        }
    }
}
