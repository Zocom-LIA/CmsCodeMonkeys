
namespace CodeMonkeys.CMS.Public.Components.Pages.GridLayout
{
public class TodoService
{
    public List<TodoItem> ToolbarTodos { get; set; } = new List<TodoItem>();
    public List<TodoItem> TodoList1 { get; set; } = new List<TodoItem>();
    public List<TodoItem> TodoList2 { get; set; } = new List<TodoItem>();

    public void AddTodoToToolbar(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            ToolbarTodos.Add(new TodoItem { Text = text });
        }
    }

public void MoveTodo(TodoItem todo, string target)
{
    // Ta bort från den aktuella listan
    if (ToolbarTodos.Contains(todo))
    {
        ToolbarTodos.Remove(todo);
    }
    else if (TodoList1.Contains(todo))
    {
        TodoList1.Remove(todo);
    }
    else if (TodoList2.Contains(todo))
    {
        TodoList2.Remove(todo);
    }

    // Lägg till i den nya listan
    switch (target)
    {
        case "toolbar":
            ToolbarTodos.Add(todo);
            break;
        case "box1":
            TodoList1.Add(todo);
            break;
        case "box2":
            TodoList2.Add(todo);
            break;
    }
}


    private void RemoveTodoFromAllLists(TodoItem todoItem)
    {
        ToolbarTodos.Remove(todoItem);
        TodoList1.Remove(todoItem);
        TodoList2.Remove(todoItem);
    }

    public List<TodoItem> GetTodosForBox(string boxName)
    {
        return boxName switch
        {
            "box1" => TodoList1,
            "box2" => TodoList2,
            "toolbar" => ToolbarTodos,
            _ => throw new ArgumentException("Invalid box name")
        };
    }
}
}