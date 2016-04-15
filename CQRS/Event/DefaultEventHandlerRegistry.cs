using CQRS.Handling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Event
{
    public class DefaultEventHandlerRegistry : IEventHandlerRegistry
    {
        private Type _genericHandlerType;
        private HandlerRegistry<IEventHandler, IEvent> _handlerRegistry;

        public DefaultEventHandlerRegistry()
        {
            _genericHandlerType = typeof(IEventHandler<>);
            _handlerRegistry = new HandlerRegistry<IEventHandler, IEvent>();
        }

        public IEventHandler GetEventHandler(IEvent e)
        {
            return _handlerRegistry.GetHandler(e);
        }

        public void RegisterEventHandler(IEventHandler handler)
        {
            var supportedEventTypes = GetSupportedEventTypes(handler);
            foreach (var supportedEventType in supportedEventTypes)
            {
                _handlerRegistry.RegisterHandler(supportedEventType, handler);
            }
        }

        private IEnumerable<Type> GetSupportedEventTypes(IEventHandler handler)
        {
            return handler.GetType().GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == _genericHandlerType)
                .Select(h => h.GetGenericArguments()[0]);
        }
    }
}
