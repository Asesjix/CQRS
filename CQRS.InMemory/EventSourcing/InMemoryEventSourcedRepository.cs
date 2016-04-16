using CQRS.Event;
using CQRS.EventSourcing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.InMemory.EventSourcing
{
    public class InMemoryEventSourcedRepository<TEventSourced> : IEventSourcedRepository<TEventSourced>
        where TEventSourced : IEventSourced
    {
        private readonly ConcurrentDictionary<Guid, TEventSourced> _store;
        private readonly IEventBus _eventBus;

        public InMemoryEventSourcedRepository(IEventBus eventBus)
        {
            _store = new ConcurrentDictionary<Guid, TEventSourced>();
            _eventBus = eventBus;
        }

        public async Task<TEventSourced> FindAsync(Guid id)
        {
            var eventSourced = default(TEventSourced);
            _store.TryGetValue(id, out eventSourced);
            return await Task.FromResult(eventSourced);
        }

        public async Task<TEventSourced> GetAsync(Guid id)
        {
            var eventSourced = await FindAsync(id);
            if (eventSourced == null)
            {
                throw new KeyNotFoundException();
            }
            return eventSourced;
        }

        public async Task SaveAsync(TEventSourced eventSourced, Guid correlationId)
        {
            _store.AddOrUpdate(eventSourced.Id, eventSourced, (id, storedValue) => eventSourced);

            foreach (var e in eventSourced.Events)
            {
                await _eventBus.PublishAsync(e);
            }
        }
    }
}
