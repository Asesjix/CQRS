using System;
using System.Collections.Generic;

namespace CQRS.EventSourcing
{
    public interface IEventSourced
    {
        Guid Id { get; }
        int Version { get; }
        IEnumerable<IVersionedEvent> Events { get; }
    }
}
