using System;

namespace CQRS.Messaging
{
    public class Message : IMessage
    {
        public Message(IMessagePayload payload)
        {
            Id = Guid.NewGuid();
            Payload = payload;
        }

        public Guid Id { get; private set; }
        public IMessagePayload Payload { get; private set; }
    }
}
