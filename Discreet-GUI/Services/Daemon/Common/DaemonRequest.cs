using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Services.Daemon.Common
{
    /// <summary>
    /// Abstract base request class that other concrete Daemon request method classes can inherit
    /// </summary>
    public class DaemonRequest
    {
        [JsonPropertyName("jsonrpc")]
        public string RpcVersion { get => "2.0"; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("params")]
        public object[] Parameters { get; set; }

        public DaemonRequest() { }

        public DaemonRequest(string method, params object[] parameters)
        {
            Method = method;
            Id = new Random().Next(0, 1000);
            Parameters = parameters;
        }

        public string Serialize() => JsonSerializer.Serialize(this);
        public static DaemonRequest Deserialize(string json) => JsonSerializer.Deserialize<DaemonRequest>(json);
    }
}
