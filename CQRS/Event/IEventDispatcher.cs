using System.Threading.Tasks;

namespace CQRS.Event
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(IEvent e);
    }
}
