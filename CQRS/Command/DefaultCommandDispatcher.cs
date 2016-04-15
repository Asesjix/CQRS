using System.Threading.Tasks;

namespace CQRS.Command
{
    public class DefaultCommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerRegistry _handlerRegistry;

        public DefaultCommandDispatcher(ICommandHandlerRegistry handlerRegistry)
        {
            _handlerRegistry = handlerRegistry;
        }

        public async Task DispatchAsync(ICommand c)
        {
            var handler = _handlerRegistry.GetCommandHandler(c);
            await ((dynamic)handler).HandleAsync((dynamic)c);
        }
    }
}
