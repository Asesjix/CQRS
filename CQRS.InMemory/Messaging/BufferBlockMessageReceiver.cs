using CQRS.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.InMemory.Messaging
{
    public class BufferBlockMessageReceiver : IMessageReceiver
    {
        private readonly IMessageBus _messageBus;
        private CancellationTokenSource _cancellationTokenSource;

        public BufferBlockMessageReceiver(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void Start()
        {
            if (_cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(() => ReceiveMessage(_cancellationTokenSource.Token));
            }
        }
        /*
        private async Task ReceiveMessages(CancellationToken cancellationToken)
        {
            await ReceiveMessageAsync(cancellationToken);
        }
        */
        private void ReceiveMessage(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested == false)
            {
                var message = _messageBus.Receive();
                Task.Factory.StartNew(() => ReceiveMessage(cancellationToken));
                MessageReceived(this, new MessageReceivedEventArgs(message));
            }
        }

        public void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }
        }
    }
}
