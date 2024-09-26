using System;
using System.Collections.Generic;

namespace CodeMonkeys.CMS.Public.Components.Pages.DragAndDropFlashy2
{
    public class TodoService2
    {
        // Todo lists
        public List<TodoItem2> TodoList1 { get; set; } = new List<TodoItem2>();
        public List<TodoItem2> TodoList2 { get; set; } = new List<TodoItem2>();
        public List<TodoItem2> TodoList3 { get; set; } = new List<TodoItem2>();
        public List<TodoItem2> TodoList4 { get; set; } = new List<TodoItem2>();

        // Box background colors
        public string Box1Color { get; set; } = "White";
        public string Box2Color { get; set; } = "White";
        public string Box3Color { get; set; } = "White"; // For Toolbar
        public string Box4Color { get; set; } = "White";
        private TodoItem2? draggedTodoItem;

        // Add a new todo item to a specific list
        public void AddTodo(int listNumber, string text)
        {
            var newTodo = new TodoItem2 { Text = text, FontSize = 16 }; 
            switch (listNumber)
            {
                case 1:
                    TodoList1.Add(newTodo);
                    break;
                case 2:
                    TodoList2.Add(newTodo);
                    break;
                case 3:
                    TodoList3.Add(newTodo);
                    break;
                case 4:
                    TodoList4.Add(newTodo);
                    break;
            }
        }

        // Remove a todo item from all lists
        public void RemoveTodo(TodoItem2 todo)
        {
            TodoList1.Remove(todo);
            TodoList2.Remove(todo);
            TodoList3.Remove(todo);
            TodoList4.Remove(todo);
        }

        // Start dragging a todo item
        public void StartDrag(TodoItem2 todo)
        {
            draggedTodoItem = todo;
        }

        // Drop the dragged todo item into the target list
        public void DropTodoItem(int targetListNumber)
        {
            if (draggedTodoItem != null)
            {
                TodoList1.Remove(draggedTodoItem);
                TodoList2.Remove(draggedTodoItem);
                TodoList3.Remove(draggedTodoItem);
                TodoList4.Remove(draggedTodoItem);

                switch (targetListNumber)
                {
                    case 1:
                        TodoList1.Add(draggedTodoItem);
                        break;
                    case 2:
                        TodoList2.Add(draggedTodoItem);
                        break;
                    case 3:
                        TodoList3.Add(draggedTodoItem);
                        break;
                    case 4:
                        TodoList4.Add(draggedTodoItem);
                        break;
                }
                draggedTodoItem = null;
            }
        }

        // Save the background color for a specific box
        public void SaveBoxColor(int boxNumber, string color)
        {
            switch (boxNumber)
            {
                case 1:
                    Box1Color = color;
                    break;
                case 2:
                    Box2Color = color;
                    break;
                case 3:
                    Box3Color = color;
                    break;
                case 4:
                    Box4Color = color;
                    break;
            }
        }
    }
}
