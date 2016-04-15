using CQRS.Messaging;
using System.Threading.Tasks;

namespace CQRS.Command
{
    public class DefaultCommandBus : ICommandBus
    {
        private readonly IMessageSender _messageSender;

        public DefaultCommandBus(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task SendAsync(ICommand c)
        {
            var message = BuildMessage(c);
            await _messageSender.SendAsync(message);
        }

        private IMessage BuildMessage(ICommand c)
        {
            return new Message(c);
        }
    }
}
