using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Transaction.Responses
{
    public class CreateTransactionResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("Txid")]
        public string TransactionId { get; set; }
        public string Error { get; set; }
    }
}
