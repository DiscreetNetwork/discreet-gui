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

        public bool IsOK()
        {
            try
            {
                return (Result != null && Result.GetType() == typeof(JsonElement) &&
                    ((JsonElement)Result).ValueKind == JsonValueKind.String &&
                    ((JsonElement)Result).GetString() == "OK");
            }
            catch
            {
                return false;
            }
        }
    }

    public class DaemonErrorResult
    {
        public int ErrID { get; set; }
        public string ErrMsg { get; set; }
        public object Result { get; set; }
    }
}
