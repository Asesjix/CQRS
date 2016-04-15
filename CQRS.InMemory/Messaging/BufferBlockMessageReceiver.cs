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
                
                Task.Factory.StartNew(async () =>
                {
                    await ReceiveMessagesAsync(_cancellationTokenSource.Token);
                }, TaskCreationOptions.LongRunning);
            }
        }

        private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            while (_cancellationTokenSource.Token.IsCancellationRequested == false)
            {
                var message = await _messageBus.ReceiveAsync();
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
