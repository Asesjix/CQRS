using System.Threading.Tasks;

namespace CQRS.Messaging
{
    public interface IMessageSender
    {
        Task SendAsync(IMessage m);
    }
}
