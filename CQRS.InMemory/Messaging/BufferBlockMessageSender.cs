using CQRS.Messaging;
using System.Threading.Tasks;

namespace CQRS.InMemory.Messaging
{
    public class BufferBlockMessageSender : IMessageSender
    {
        private readonly IMessageBus _messageBus;

        public BufferBlockMessageSender(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task SendAsync(IMessage m)
        {
            await _messageBus.SendAsync(m);
        }
    }
}
