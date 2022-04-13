using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ
{
    public class Subscriber
    {
        private readonly SubscriberSocket _subscriberSocket = new SubscriberSocket();
        private int _port;

        public Subscriber(IConfiguration configuration)
        {
            _port = configuration.GetValue<int>("ZMQSettings:SubscriberPort");
        }

        public void Start()
        {
            _subscriberSocket.Connect($"tcp://localhost:{_port}");
            Debug.WriteLine($"ZMQ.Subscriber: Started and listening on port: {_port}");

        }
    }
}
