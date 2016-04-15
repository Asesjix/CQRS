using System;
using System.Collections.Generic;

namespace CQRS.Handling
{
    public class HandlerRegistry<THandler, TDispatchable>
    {
        private Dictionary<Type, THandler> _handlerRegistry;

        public HandlerRegistry()
        {
            _handlerRegistry = new Dictionary<Type, THandler>();
        }

        public virtual THandler GetHandler(TDispatchable dispatchable)
        {
            var dispatchableType = dispatchable.GetType();

            var registeredHandler = default(THandler);
            _handlerRegistry.TryGetValue(dispatchableType, out registeredHandler);

            return registeredHandler;
        }

        public virtual void RegisterHandler(Type dispatchableType, THandler handler)
        {
            var registeredHandler = default(THandler);

            if (_handlerRegistry.TryGetValue(dispatchableType, out registeredHandler))
            {
                throw new InvalidOperationException(string.Format("The handler {0} cannot be registered because the handler {1} with the type {2} is already linked.",
                    handler.GetType().Name,
                    registeredHandler.GetType().Name,
                    dispatchableType.Name));
            }

            _handlerRegistry.Add(dispatchableType, handler);
        }
    }
}
