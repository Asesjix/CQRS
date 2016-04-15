using TodoList.Events;
using CQRS.Event;
using System;
using System.Threading.Tasks;

namespace TodoList.Handler
{
    public class TodoItemEventHandler :
        IEventHandler<TodoItemCreated>,
        IEventHandler<TodoItemCompleted>
    {
        public async Task HandleAsync(TodoItemCreated e)
        {
            Console.WriteLine("TodoItem {0} mit Beschreibung \"{1}\" wurde erstellt.", e.SourceId, e.Description);
            await Task.Delay(0);
        }

        public async Task HandleAsync(TodoItemCompleted e)
        {
            Console.WriteLine("TodoItem {0} wurde erledigt.", e.SourceId);
            await Task.Delay(0);
        }
    }
}
