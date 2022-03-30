using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Services.Daemon.Common;

namespace Services.Daemon.Responses
{
    public class GetMnemonicResponse : DaemonErrorResult
    {
        public string Mnemonic { get; set; }
        public string Entropy { get; set; }

        public static GetMnemonicResponse GetMnemonic(RPCServer rpcServer)
        {
            var req = new DaemonRequest("get_mnemonic");

            var resp = rpcServer.RequestUnsafe(req);

            var result = JsonSerializer.Deserialize<GetMnemonicResponse>((JsonElement)resp.Result);

            if (result.ErrMsg != null && result.ErrMsg != "")
            {
                System.Diagnostics.Debug.WriteLine("GetMnemonic : " + result.ErrMsg);
            }

            return result;
        }
    }
}
