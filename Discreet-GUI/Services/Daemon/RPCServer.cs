using Services.Daemon.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services.Daemon
{
    public class RPCServer
    {
        private readonly HttpClient _httpClient;

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
            try
            {
                var httpResponse = await _httpClient.PostAsync("/", new StringContent(req.Serialize()));
                var responseText = await httpResponse.Content.ReadAsStringAsync(); 

                if(string.IsNullOrWhiteSpace(responseText))
                {
                    return null;
                }

                var resp = DaemonResponse.Deserialize(responseText);
                
                return resp;
            }
            catch(HttpRequestException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
