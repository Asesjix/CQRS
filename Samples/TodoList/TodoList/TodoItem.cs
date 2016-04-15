using TodoList.Events;
using CQRS.EventSourcing;
using System;

namespace TodoList
{
    public class TodoItem : EventSourced
    {
        protected TodoItem(Guid id) 
            : base(id)
        {
            base.Handles<TodoItemCreated>(OnTodoItemCreated);
            base.Handles<TodoItemCompleted>(OnTodoItemCompleted);
        }

        public TodoItem(Guid id, string description)
            : this(id)
        {
            Update(new TodoItemCreated
            {
                Description = description
            });
        }

        protected string Description { get; set; }
        protected bool Done { get; set; }

        public void Complete()
        {
            Update(new TodoItemCompleted());
        }

        private void OnTodoItemCreated(TodoItemCreated e)
        {
            Done = false;
            Description = e.Description;
        }

        private void OnTodoItemCompleted(TodoItemCompleted e)
        {
            Done = true;
        }
    }
}
