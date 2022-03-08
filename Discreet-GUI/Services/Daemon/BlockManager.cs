using Services.Daemon.Common;
using Services.Daemon.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon
{
    public class BlockManager
    {
        WebClient _client;

        public BlockManager()
        {
            _client = new WebClient();
            _client.BaseAddress = "http://localhost:8350";
        }

        public GetBlockCountResponse GetBlockCount()
        {
            var request = new DaemonRequest("get_block_count");
            var responseText = _client.UploadString("/", request.Serialize());


            
            throw new NotImplementedException();
        }
    }
}
