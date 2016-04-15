using System.Threading.Tasks;

namespace CQRS.Event
{
    public interface IEventHandler
    { }

    public interface IEventHandler<T> : IEventHandler
        where T : IEvent
    {
        Task HandleAsync(T e);
    }
}
