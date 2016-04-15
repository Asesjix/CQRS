using System.Threading.Tasks;

namespace CQRS.Event
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        private readonly IEventHandlerRegistry _handlerRegistry;

        public DefaultEventDispatcher(IEventHandlerRegistry handlerRegistry)
        {
            _handlerRegistry = handlerRegistry;
        }

        public async Task DispatchAsync(IEvent e)
        {
            var handler = _handlerRegistry.GetEventHandler(e);
            await ((dynamic)handler).HandleAsync((dynamic)e);
        }
    }
}
