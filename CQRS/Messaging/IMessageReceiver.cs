using System;

namespace CQRS.Messaging
{
    public interface IMessageReceiver
    {
        void Start();
        void Stop();
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}
