using CQRS.Messaging;
using System;

namespace CQRS.Event
{
    public interface IEvent : IMessagePayload
    {
        Guid SourceId { get; set; }
    }
}
