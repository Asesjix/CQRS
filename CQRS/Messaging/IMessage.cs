using System;

namespace CQRS.Messaging
{
    public interface IMessage
    {
        Guid Id { get; }
        IMessagePayload Payload { get; }
    }
}
