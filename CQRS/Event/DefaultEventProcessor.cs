using CQRS.Messaging;

namespace CQRS.Event
{
    public class DefaultEventProcessor : IEventProcessor
    {
        private readonly IMessageReceiver _messageReceiver;
        private readonly IEventDispatcher _eventDispatcher;

        public DefaultEventProcessor(IMessageReceiver messageReceiver, IEventDispatcher eventDispatcher)
        {
            _messageReceiver = messageReceiver;
            _eventDispatcher = eventDispatcher;
        }

        public void Start()
        {
            _messageReceiver.MessageReceived += OnMessageReceived;
            _messageReceiver.Start();
        }

        public void Stop()
        {
            _messageReceiver.Stop();
            _messageReceiver.MessageReceived -= OnMessageReceived;
        }

        private async void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            var e = (IEvent)args.Message.Payload;
            await _eventDispatcher.DispatchAsync(e);
        }
    }
}
