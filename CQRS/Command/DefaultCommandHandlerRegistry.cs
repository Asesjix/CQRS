using CQRS.Handling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Command
{
    public class DefaultCommandHandlerRegistry : ICommandHandlerRegistry
    {
        private Type _genericHandlerType;
        private HandlerRegistry<ICommandHandler, ICommand> _handlerRegistry;

        public DefaultCommandHandlerRegistry()
        {
            _genericHandlerType = typeof(ICommandHandler<>);
            _handlerRegistry = new HandlerRegistry<ICommandHandler, ICommand>();
        }

        public ICommandHandler GetCommandHandler(ICommand c)
        {
            return _handlerRegistry.GetHandler(c);
        }

        public void RegisterCommandHandler(ICommandHandler handler)
        {
            var supportedCommandTypes = GetSupportedCommandTypes(handler);
            foreach (var supportedCommandType in supportedCommandTypes)
            {
                _handlerRegistry.RegisterHandler(supportedCommandType, handler);
            }
        }

        private IEnumerable<Type> GetSupportedCommandTypes(ICommandHandler handler)
        {
            return handler.GetType().GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == _genericHandlerType)
                .Select(h => h.GetGenericArguments()[0]);
        }
    }
}
