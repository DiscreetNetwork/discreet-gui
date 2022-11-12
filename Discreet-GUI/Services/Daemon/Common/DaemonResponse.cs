using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Services.Daemon.Common
{
    public class DaemonResponse
    {
        [JsonPropertyName("jsonrpc")]
        public string RpcVersion { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("result")]
        public object Result { get; set; }

        public DaemonResponse() { }

        public static DaemonResponse Deserialize(string respText) => JsonSerializer.Deserialize<DaemonResponse>(respText, new JsonSerializerOptions());

        public bool ContainsError(out DaemonErrorResponse daemonErrorResponse)
        {
            try
            {
                JsonElement element = (JsonElement)Result;
                DaemonErrorResponse error = JsonSerializer.Deserialize<DaemonErrorResponse>(element);
                daemonErrorResponse = error;
                return error.ErrID == -1;
            }
            catch (JsonException)
            {
                daemonErrorResponse = null;
                return false;
            }
        }
    }
}
