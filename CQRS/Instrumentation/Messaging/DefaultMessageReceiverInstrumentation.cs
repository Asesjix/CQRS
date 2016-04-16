using System;
using System.Diagnostics;

namespace CQRS.Instrumentation.Messaging
{
    public class DefaultMessageReceiverInstrumentation : IMessageReceiverInstrumentation
    {
        public const string TotalMessagesCounterName = "Total messages received";
        public const string TotalMessagesSuccessfullyProcessedCounterName = "Total messages processed";
        public const string TotalMessagesUnsuccessfullyProcessedCounterName = "Total messages processed (fails)";
        public const string TotalMessagesCompletedCounterName = "Total messages completed";
        public const string TotalMessagesNotCompletedCounterName = "Total messages not completed";
        public const string MessagesReceivedPerSecondCounterName = "Messages received/sec";
        public const string AverageMessageProcessingTimeCounterName = "Average message processing time";
        public const string AverageMessageProcessingTimeBaseCounterName = "Average message processing time base";
        public const string CurrentMessagesInProcessCounterName = "Current messages";

        private readonly string _instanceName;
        private readonly bool _instrumentationEnabled;
        private readonly string _categoryName;

        private readonly PerformanceCounter _totalMessagesCounter;
        private readonly PerformanceCounter _totalMessagesSuccessfullyProcessedCounter;
        private readonly PerformanceCounter _totalMessagesUnsuccessfullyProcessedCounter;
        private readonly PerformanceCounter _totalMessagesCompletedCounter;
        private readonly PerformanceCounter _totalMessagesNotCompletedCounter;
        private readonly PerformanceCounter _messagesReceivedPerSecondCounter;
        private readonly PerformanceCounter _averageMessageProcessingTimeCounter;
        private readonly PerformanceCounter _averageMessageProcessingTimeBaseCounter;
        private readonly PerformanceCounter _currentMessagesInProcessCounter;

        public DefaultMessageReceiverInstrumentation(string instanceName, bool instrumentationEnabled)
        {
            _instanceName = instanceName;
            _instrumentationEnabled = instrumentationEnabled;
            _categoryName = string.Format("CQRS {0} Receivers", instanceName);

            if (_instrumentationEnabled)
            {
                SetupCategory();

                _totalMessagesCounter = new PerformanceCounter(_categoryName, TotalMessagesCounterName, _instanceName, false);
                _totalMessagesSuccessfullyProcessedCounter = new PerformanceCounter(_categoryName, TotalMessagesSuccessfullyProcessedCounterName, _instanceName, false);
                _totalMessagesUnsuccessfullyProcessedCounter = new PerformanceCounter(_categoryName, TotalMessagesUnsuccessfullyProcessedCounterName, _instanceName, false);
                _totalMessagesCompletedCounter = new PerformanceCounter(_categoryName, TotalMessagesCompletedCounterName, _instanceName, false);
                _totalMessagesNotCompletedCounter = new PerformanceCounter(_categoryName, TotalMessagesNotCompletedCounterName, _instanceName, false);
                _messagesReceivedPerSecondCounter = new PerformanceCounter(_categoryName, MessagesReceivedPerSecondCounterName, _instanceName, false);
                _averageMessageProcessingTimeCounter = new PerformanceCounter(_categoryName, AverageMessageProcessingTimeCounterName, _instanceName, false);
                _averageMessageProcessingTimeBaseCounter = new PerformanceCounter(_categoryName, AverageMessageProcessingTimeBaseCounterName, _instanceName, false);
                _currentMessagesInProcessCounter = new PerformanceCounter(_categoryName, CurrentMessagesInProcessCounterName, _instanceName, false);

                _totalMessagesCounter.RawValue = 0;
                _totalMessagesSuccessfullyProcessedCounter.RawValue = 0;
                _totalMessagesUnsuccessfullyProcessedCounter.RawValue = 0;
                _totalMessagesCompletedCounter.RawValue = 0;
                _totalMessagesNotCompletedCounter.RawValue = 0;
                _averageMessageProcessingTimeCounter.RawValue = 0;
                _averageMessageProcessingTimeBaseCounter.RawValue = 0;
                _currentMessagesInProcessCounter.RawValue = 0;
            }
        }

        private void SetupCategory()
        {
            if (PerformanceCounterCategory.Exists(_categoryName) == false)
            {
                CounterCreationDataCollection counterDataCollection = new CounterCreationDataCollection();
                counterDataCollection.Add(new CounterCreationData(CurrentMessagesInProcessCounterName, string.Empty, PerformanceCounterType.NumberOfItems32));
                counterDataCollection.Add(new CounterCreationData(TotalMessagesCounterName, string.Empty, PerformanceCounterType.NumberOfItems32));
                counterDataCollection.Add(new CounterCreationData(TotalMessagesSuccessfullyProcessedCounterName, string.Empty, PerformanceCounterType.NumberOfItems32));
                counterDataCollection.Add(new CounterCreationData(TotalMessagesUnsuccessfullyProcessedCounterName, string.Empty, PerformanceCounterType.NumberOfItems32));
                counterDataCollection.Add(new CounterCreationData(TotalMessagesCompletedCounterName, string.Empty, PerformanceCounterType.NumberOfItems32));
                counterDataCollection.Add(new CounterCreationData(TotalMessagesNotCompletedCounterName, string.Empty, PerformanceCounterType.NumberOfItems32));
                counterDataCollection.Add(new CounterCreationData(AverageMessageProcessingTimeCounterName, string.Empty, PerformanceCounterType.RawFraction));
                counterDataCollection.Add(new CounterCreationData(AverageMessageProcessingTimeBaseCounterName, string.Empty, PerformanceCounterType.RawBase));
                counterDataCollection.Add(new CounterCreationData(MessagesReceivedPerSecondCounterName, string.Empty, PerformanceCounterType.RateOfCountsPerSecond32));

                /// Need admin rights
                PerformanceCounterCategory.Create(_categoryName, "", PerformanceCounterCategoryType.MultiInstance, counterDataCollection);
            }
        }

        protected string InstanceName
        {
            get { return _instanceName; }
        }

        protected bool InstrumentationEnabled
        {
            get { return _instrumentationEnabled; }
        }

        public void MessageReceived()
        {
            if (InstrumentationEnabled)
            {
                try
                {
                    _totalMessagesCounter.Increment();
                    _messagesReceivedPerSecondCounter.Increment();
                    _currentMessagesInProcessCounter.Increment();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        public void MessageProcessed(bool success, long elapsedMilliseconds)
        {
            if (InstrumentationEnabled)
            {
                try
                {
                    if (success)
                    {
                        _totalMessagesSuccessfullyProcessedCounter.Increment();
                    }
                    else
                    {
                        _totalMessagesUnsuccessfullyProcessedCounter.Increment();
                    }

                    _averageMessageProcessingTimeCounter.IncrementBy(elapsedMilliseconds / 100);
                    _averageMessageProcessingTimeBaseCounter.Increment();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        public void MessageCompleted(bool success)
        {
            if (InstrumentationEnabled)
            {
                try
                {
                    if (success)
                    {
                        _totalMessagesCompletedCounter.Increment();
                    }
                    else
                    {
                        _totalMessagesNotCompletedCounter.Increment();
                    }
                    _currentMessagesInProcessCounter.Decrement();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }
    }
}
