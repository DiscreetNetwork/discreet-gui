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
        private readonly HttpClient _httpClient;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public RPCServer(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("http://localhost:8350");
            _httpClient = httpClient;
        }

        /// <summary>
        /// Sends a request asynchronously.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<DaemonResponse> Request(DaemonRequest req)
        {
            await _semaphore.WaitAsync();

            var httpResponse = await _httpClient.PostAsync("/", new StringContent(req.Serialize()));
            var responseText = await httpResponse.Content.ReadAsStringAsync(); 

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
            var requestTask = _httpClient.PostAsync("/", new StringContent(req.Serialize()));
            var responseResult = requestTask.Result;
            var responseText = responseResult.Content.ReadAsStringAsync().Result;

            var resp = DaemonResponse.Deserialize(responseText);

            return resp;
        }
    }
}
