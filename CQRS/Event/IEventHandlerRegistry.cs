namespace CQRS.Event
{
    public interface IEventHandlerRegistry
    {
        IEventHandler GetEventHandler(IEvent e);
        void RegisterEventHandler(IEventHandler handler);
    }
}
