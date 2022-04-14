using Services.ZMQ.Handlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ.Registries.Common
{
    public interface IMessageHandlerRegistry
    {
        /// <summary>
        /// Should return a 'MessageHandler' based on a string key
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        MessageHandler GetHandler(string topic);

        List<string> GetKeys();
    }
}
