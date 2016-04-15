using System.Threading.Tasks;

namespace CQRS.Event
{
    public interface IEventBus
    {
        Task PublishAsync(IEvent e);
    }
}
