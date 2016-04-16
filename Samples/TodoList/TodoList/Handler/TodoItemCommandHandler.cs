using TodoList.Commands;
using CQRS.Command;
using CQRS.EventSourcing;
using System.Threading.Tasks;

namespace TodoList.Handler
{
    public class TodoItemCommandHandler :
        ICommandHandler<CreateTodoItem>,
        ICommandHandler<CompleteTodoItem>
    {
        private readonly IEventSourcedRepository<TodoItem> _todoItemRepository;

        public TodoItemCommandHandler(IEventSourcedRepository<TodoItem> todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task HandleAsync(CreateTodoItem c)
        {
            await Task.Delay(50);
            var todoItem = new TodoItem(c.TodoItemId, c.Description);
            await _todoItemRepository.SaveAsync(todoItem, c.Id);
        }

        public async Task HandleAsync(CompleteTodoItem c)
        {
            await Task.Delay(50);
            var todoItem = await _todoItemRepository.GetAsync(c.TodoItemId);
            todoItem.Complete();
            await _todoItemRepository.SaveAsync(todoItem, c.Id);
        }
    }
}
