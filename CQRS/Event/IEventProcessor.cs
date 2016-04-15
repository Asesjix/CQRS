namespace CQRS.Event
{
    public interface IEventProcessor
    {
        void Start();
        void Stop();
    }
}
