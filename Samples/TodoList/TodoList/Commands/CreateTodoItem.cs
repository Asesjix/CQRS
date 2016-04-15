using CQRS.Command;
using System;

namespace TodoList.Commands
{
    public class CreateTodoItem : ICommand
    {
        public CreateTodoItem(Guid todoItemId, string description)
        {
            Id = Guid.NewGuid();
            TodoItemId = todoItemId;
            Description = description;
        }

        public Guid Id { get; private set; }
        public Guid TodoItemId { get; private set; }
        public string Description { get; private set; }
    }
}
