using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Services.Daemon.Common
{
    public class DaemonResponse
    {
        public string RpcVersion { get; set; }
        public int Id { get; set; }
        public object Result { get; set; }
    }

    public class DaemonErrorResult
    {
        public int ErrID { get; set; }
        public string ErrMsg { get; set; }
        public object Result { get; set; }
    }
}
