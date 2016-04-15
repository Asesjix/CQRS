using CQRS.Event;

namespace CQRS.EventSourcing
{
    public interface IVersionedEvent : IEvent
    {
        int Version { get; }
    }
}
