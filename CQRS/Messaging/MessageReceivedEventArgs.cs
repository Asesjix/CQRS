using System;

namespace CQRS.Messaging
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(IMessage message)
        {
            Message = message;
        }

        public IMessage Message { get; private set; }
    }
}
