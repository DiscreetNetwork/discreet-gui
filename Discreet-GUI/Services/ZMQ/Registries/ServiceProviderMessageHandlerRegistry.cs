using Microsoft.Extensions.DependencyInjection;
using Services.ZMQ.Handlers.Common;
using Services.ZMQ.Registries.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ.Registries
{
    public class ServiceProviderMessageHandlerRegistry : IMessageHandlerRegistry
    {
        private readonly List<Type> _handlerTypes = new List<Type>();
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderMessageHandlerRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var handlers = typeof(ServiceProviderMessageHandlerRegistry).Assembly.GetTypes().Where(t => t.BaseType == typeof(MessageHandler));
            foreach (var handler in handlers)
            {
                _handlerTypes.Add(handler);
            }
        }

        public MessageHandler GetHandler(string topic)
        {
            Type handlerType = _handlerTypes.Where(t => t.Name.ToLower().Replace("handler", "") == topic).FirstOrDefault();
            if (handlerType is null) throw new Exception($"ZMQ.Registries - ServiceCollectionMessageHandlerRegistry: handler for topic {topic} were not found");

            return _serviceProvider.GetRequiredService(handlerType) as MessageHandler;
        }

        public List<string> GetKeys()
        {
            return _handlerTypes.Select(t => t.Name.ToLower().Replace("handler", "")).ToList();
        }
    }
}
