using CQRS.Command;
using System;

namespace TodoList.Commands
{
    public class CompleteTodoItem : ICommand
    {
        public CompleteTodoItem(Guid todoItemId)
        {
            Id = Guid.NewGuid();
            TodoItemId = todoItemId;
        }

        public Guid Id { get; private set; }
        public Guid TodoItemId { get; private set; }
    }
}
