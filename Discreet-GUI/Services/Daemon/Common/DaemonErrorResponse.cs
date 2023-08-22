using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Daemon.Common
{
    public class DaemonErrorResponse
    {
        [JsonPropertyName("code")]
        public int ErrID { get; set; }
        [JsonPropertyName("message")]
        public string ErrMsg { get; set; }
        [JsonPropertyName("data")]
        public object Result { get; set; }
    }
}
