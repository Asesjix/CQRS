namespace CQRS.Instrumentation.Messaging
{
    public interface IMessageReceiverInstrumentation
    {
        void MessageCompleted(bool success);
        void MessageProcessed(bool success, long elapsedMilliseconds);
        void MessageReceived();
    }
}
