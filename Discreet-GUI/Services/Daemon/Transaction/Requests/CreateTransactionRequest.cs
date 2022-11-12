using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Transaction.Requests
{
    public class CreateTransactionParam
    {
        public string To { get; set; }

        public ulong Amount { get; set; }
    }

    public class CreateTransactionRequest
    {
        public string Address { get; set; }
        public List<CreateTransactionParam> Params { get; set; }
        public bool? Raw { get; set; }
        public bool? Relay { get; set; }
    }
}
