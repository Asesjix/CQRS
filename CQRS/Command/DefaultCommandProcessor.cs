using CQRS.Messaging;

namespace CQRS.Command
{
    public class DefaultCommandProcessor : ICommandProcessor
    {
        private readonly IMessageReceiver _messageReceiver;
        private readonly ICommandDispatcher _commandDispatcher;

        public DefaultCommandProcessor(IMessageReceiver messageReceiver, ICommandDispatcher commandDispatcher)
        {
            _messageReceiver = messageReceiver;
            _commandDispatcher = commandDispatcher;
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
            var c = (ICommand)args.Message.Payload;
            await _commandDispatcher.DispatchAsync(c);
        }
    }
}
