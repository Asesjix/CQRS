using System.Threading.Tasks;

namespace CQRS.Command
{
    public interface ICommandBus
    {
        Task SendAsync(ICommand c);
    }
}
