using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NetMQ;
using NetMQ.Sockets;
using Services.ZMQ.Registries.Common;
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
        private readonly IMessageHandlerRegistry _messageHandlerRegistry;
        private readonly SubscriberSocket _subscriberSocket = new SubscriberSocket();
        private int _port;

        public Subscriber(IConfiguration configuration, IMessageHandlerRegistry messageHandlerRegistry)
        {
            _port = configuration.GetValue<int>("ZMQSettings:SubscriberPort");
            _messageHandlerRegistry = messageHandlerRegistry;
        }

        public void Start()
        {
            _subscriberSocket.Connect($"tcp://localhost:{_port}");
            Debug.WriteLine($"ZMQ.Subscriber: Started and listening on port: {_port}");

            foreach (var topic in _messageHandlerRegistry.GetKeys())
            {
                _subscriberSocket.Subscribe(topic);
            }

            while (true)
            {
                var topicBytes = _subscriberSocket.ReceiveFrameBytes();
                var messageBytes = _subscriberSocket.ReceiveFrameBytes();

                Task.Factory.StartNew(async () => await _messageHandlerRegistry.GetHandler(Encoding.UTF8.GetString(topicBytes)).Handle(Encoding.UTF8.GetString(messageBytes)));
            }
        }
    }
}
