namespace CQRS.Command
{
    public interface ICommandHandlerRegistry
    {
        ICommandHandler GetCommandHandler(ICommand c);
        void RegisterCommandHandler(ICommandHandler handler);
    }
}
