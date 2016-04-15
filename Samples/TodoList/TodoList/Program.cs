using CQRS.Command;
using CQRS.Event;
using CQRS.EventSourcing;
using CQRS.InMemory.EventSourcing;
using CQRS.InMemory.Messaging;
using CQRS.Messaging;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using TodoList.Commands;
using TodoList.Handler;

namespace TodoList
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = SetupUnityContainer();
            var commandProcessor = container.Resolve<ICommandProcessor>();
            var eventProcessor = container.Resolve<IEventProcessor>();

            commandProcessor.Start();
            eventProcessor.Start();

            var todoListAgent = new TodoListAgent(container);
            var workTask = todoListAgent.WorkAsync();
            Task.WaitAll(workTask, Task.Delay(TimeSpan.FromMinutes(1)));

            commandProcessor.Stop();
            eventProcessor.Stop();

            Console.WriteLine("Done...");
            Console.ReadKey();
        }

        private static IUnityContainer SetupUnityContainer()
        {
            var container = new UnityContainer();

            /// Register Event Stack
            container.RegisterType<TodoItemEventHandler>();
            var eventHandlerRegistry = new DefaultEventHandlerRegistry();
            var todoItemEventHandler = container.Resolve<TodoItemEventHandler>();
            eventHandlerRegistry.RegisterEventHandler(todoItemEventHandler);
            container.RegisterInstance<IEventHandlerRegistry>(eventHandlerRegistry);

            var eventDispatcher = new DefaultEventDispatcher(eventHandlerRegistry);
            container.RegisterInstance<IEventDispatcher>(eventDispatcher);

            var eventMessageBus = new BufferBlockMessageBus();
            container.RegisterInstance<IMessageBus>("EventMessageBus", eventMessageBus);
            var eventMessageSender = new BufferBlockMessageSender(eventMessageBus);
            container.RegisterInstance<IMessageSender>("EventMessageSender", eventMessageSender);
            var eventMessageReceiver = new BufferBlockMessageReceiver(eventMessageBus);
            container.RegisterInstance<IMessageReceiver>("EventMessageReceiver", eventMessageReceiver);

            var eventBus = new DefaultEventBus(eventMessageSender);
            container.RegisterInstance<IEventBus>(eventBus);

            var eventProcessor = new DefaultEventProcessor(eventMessageReceiver, eventDispatcher);
            container.RegisterInstance<IEventProcessor>(eventProcessor);

            /// Register EventSourcing
            var todoItemEventSourcedRepository = new InMemoryEventSourcedRepository<TodoItem>(eventBus);
            container.RegisterInstance<IEventSourcedRepository<TodoItem>>(todoItemEventSourcedRepository);

            /// Register Command Stack
            container.RegisterType<TodoItemCommandHandler>();
            var commandHandlerRegistry = new DefaultCommandHandlerRegistry();
            var todoItemCommandHandler = container.Resolve<TodoItemCommandHandler>();
            commandHandlerRegistry.RegisterCommandHandler(todoItemCommandHandler);
            container.RegisterInstance<ICommandHandlerRegistry>(commandHandlerRegistry);

            var commandDispatcher = new DefaultCommandDispatcher(commandHandlerRegistry);
            container.RegisterInstance<ICommandDispatcher>(commandDispatcher);

            var commandMessageBus = new BufferBlockMessageBus();
            container.RegisterInstance<IMessageBus>("CommandMessageBus", commandMessageBus);
            var commandMessageSender = new BufferBlockMessageSender(commandMessageBus);
            container.RegisterInstance<IMessageSender>("CommandMessageSender", commandMessageSender);
            var commandMessageReceiver = new BufferBlockMessageReceiver(commandMessageBus);
            container.RegisterInstance<IMessageReceiver>("CommandMessageReceiver", commandMessageReceiver);

            var commandBus = new DefaultCommandBus(commandMessageSender);
            container.RegisterInstance<ICommandBus>(commandBus);

            var commandProcessor = new DefaultCommandProcessor(commandMessageReceiver, commandDispatcher);
            container.RegisterInstance<ICommandProcessor>(commandProcessor);

            return container;
        }
    }

    class TodoListAgent
    {
        private ICommandBus _commandBus;

        public TodoListAgent(IUnityContainer container)
        {
            _commandBus = container.Resolve<ICommandBus>();
        }

        public async Task WorkAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var todoItemId = Guid.NewGuid();
                await _commandBus.SendAsync(new CreateTodoItem(todoItemId, string.Format("My Todo #{0}", i + 1)));
                ///await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}
