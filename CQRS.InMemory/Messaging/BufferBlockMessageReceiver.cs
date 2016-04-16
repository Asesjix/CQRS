using CQRS.Messaging;
using CQRS.Threading;
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

                var scheduler = new IOCompletionPortTaskScheduler(16, 8);
                //var factory = new TaskFactory(scheduler);
                var factory = Task.Factory;
                factory.StartNew(() =>
                {
                    ReceiveMessage(_cancellationTokenSource.Token);
                }, TaskCreationOptions.LongRunning);
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
            var message = _messageBus.Receive();
            Task.Factory.StartNew(() => ReceiveNextMessage(cancellationToken));
            MessageReceived(this, new MessageReceivedEventArgs(message));
        }

        private void ReceiveNextMessage(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested == false)
            {
                Task.Factory.StartNew(() => ReceiveMessage(cancellationToken));
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
