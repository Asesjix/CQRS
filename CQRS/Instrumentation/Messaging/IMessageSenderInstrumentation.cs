namespace CQRS.Instrumentation.Messaging
{
    public interface IMessageSenderInstrumentation
    {
        void MessageSend();
        void MessageSended(bool success, long elapsedMilliseconds);
    }
}
