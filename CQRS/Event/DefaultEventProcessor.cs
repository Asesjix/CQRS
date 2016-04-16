using CQRS.Instrumentation.Messaging;
using CQRS.Messaging;
using System.Diagnostics;

namespace CQRS.Event
{
    public class DefaultEventProcessor : IEventProcessor
    {
        private readonly IMessageReceiver _messageReceiver;
        private readonly IMessageReceiverInstrumentation _messageReceiverInstrumentation;
        private readonly IEventDispatcher _eventDispatcher;

        public DefaultEventProcessor(IMessageReceiver messageReceiver, IMessageReceiverInstrumentation messageReceiverInstrumentation, IEventDispatcher eventDispatcher)
        {
            _messageReceiver = messageReceiver;
            _messageReceiverInstrumentation = messageReceiverInstrumentation;
            _eventDispatcher = eventDispatcher;
        }

        public void Start()
        {
            _messageReceiver.MessageReceived += OnMessageReceivedAsync;
            _messageReceiver.Start();
        }

        public void Stop()
        {
            _messageReceiver.Stop();
            _messageReceiver.MessageReceived -= OnMessageReceivedAsync;
        }

        private async void OnMessageReceivedAsync(object sender, MessageReceivedEventArgs args)
        {
            _messageReceiverInstrumentation.MessageReceived();

            var processedCounter = Stopwatch.StartNew();
            var e = (IEvent)args.Message.Payload;
            processedCounter.Stop();
            _messageReceiverInstrumentation.MessageProcessed(true, processedCounter.ElapsedMilliseconds);
            
            await _eventDispatcher.DispatchAsync(e);
            _messageReceiverInstrumentation.MessageCompleted(true);
        }
    }
}
