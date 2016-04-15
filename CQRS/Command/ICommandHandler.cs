using System.Threading.Tasks;

namespace CQRS.Command
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<T> : ICommandHandler
        where T : ICommand
    {
        Task HandleAsync(T c);
    }
}
