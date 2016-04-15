using CQRS.EventSourcing;

namespace TodoList.Events
{
    public class TodoItemCreated : VersionedEvent
    {
        public string Description { get; set; }
    }
}
