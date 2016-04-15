using System.Threading.Tasks;

namespace CQRS.Command
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync(ICommand c);
    }
}
