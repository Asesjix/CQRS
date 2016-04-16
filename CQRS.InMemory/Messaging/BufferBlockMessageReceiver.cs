using CQRS.Messaging;
using System;
using System.Threading;

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

        public async void Start()
        {
            if (_cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();

                while (_cancellationTokenSource.Token.IsCancellationRequested == false)
                {
                    var message = await _messageBus.ReceiveAsync();
                    MessageReceived(this, new MessageReceivedEventArgs(message));
                }
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
