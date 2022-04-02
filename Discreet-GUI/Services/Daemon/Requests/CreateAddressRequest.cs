using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Daemon.Requests
{
    public class CreateAddressRequest
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string Label { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string Type { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public bool? Deterministic { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Secret { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Spend { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string View { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ScanForBalance { get; set; }
    }
}
