using CQRS.Messaging;
using System.Threading.Tasks;

namespace CQRS.Event
{
    public class DefaultEventBus : IEventBus
    {
        private readonly IMessageSender _messageSender;

        public DefaultEventBus(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task PublishAsync(IEvent e)
        {
            var message = BuildMessage(e);
            await _messageSender.SendAsync(message);
        }

        private IMessage BuildMessage(IEvent e)
        {
            return new Message(e);
        }
    }
}
