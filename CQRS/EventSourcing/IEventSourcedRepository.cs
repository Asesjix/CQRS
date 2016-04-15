using System;
using System.Threading.Tasks;

namespace CQRS.EventSourcing
{
    public interface IEventSourcedRepository<TEventSourced>
        where TEventSourced : IEventSourced
    {
        Task<TEventSourced> FindAsync(Guid id);
        Task<TEventSourced> GetAsync(Guid id);
        Task SaveAsync(TEventSourced eventSourced, Guid correlationId);
    }
}
