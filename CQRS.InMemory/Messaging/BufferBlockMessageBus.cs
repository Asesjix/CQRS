using CQRS.Messaging;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CQRS.InMemory.Messaging
{
    public class BufferBlockMessageBus : IMessageBus
    {
        private readonly BufferBlock<IMessage> _bufferBlock;

        public BufferBlockMessageBus()
        {
            _bufferBlock = new BufferBlock<IMessage>();
        }

        public IMessage Receive()
        {
            return _bufferBlock.Receive();
        }

        public async Task<IMessage> ReceiveAsync()
        {
            return await _bufferBlock.ReceiveAsync();
        }

        public async Task SendAsync(IMessage m)
        {
            await _bufferBlock.SendAsync(m);
        }
    }
}
