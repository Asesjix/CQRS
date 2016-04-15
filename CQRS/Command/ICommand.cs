using CQRS.Messaging;
using System;

namespace CQRS.Command
{
    public interface ICommand : IMessagePayload
    {
        Guid Id { get; }
    }
}
