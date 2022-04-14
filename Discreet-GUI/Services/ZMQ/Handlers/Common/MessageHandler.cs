using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ.Handlers.Common
{
    public abstract class MessageHandler
    {
        public abstract void Handle(string message);
    }
}
