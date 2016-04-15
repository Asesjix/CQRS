namespace CQRS.Command
{
    public interface ICommandProcessor
    {
        void Start();
        void Stop();
    }
}
