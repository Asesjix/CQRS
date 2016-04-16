using CQRS.Messaging;
using System.Threading.Tasks;

namespace CQRS.InMemory.Messaging
{
    public interface IMessageBus
    {
        IMessage Receive();
        Task<IMessage> ReceiveAsync();
        Task SendAsync(IMessage m);
    }
}
