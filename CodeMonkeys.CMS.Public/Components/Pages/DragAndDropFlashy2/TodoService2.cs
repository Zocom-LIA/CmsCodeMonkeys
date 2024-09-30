using System;
using System.Collections.Generic;

namespace CodeMonkeys.CMS.Public.Components.Pages.DragAndDropFlashy2
{
    public class Box
    {
        public string Color { get; set; }
        public string Name { get; set; }
    }

    public class ContentItemService
    {
        private List<ContentItem>[] _items;
        private Box[] _boxes = new Box[4];
        private ContentItem? draggedContentItem;

        public ContentItemService()
        {
            _items = new List<ContentItem>[4];
            for (int i = 0; i < 4; i++)
            {
                _items[i] = new();
            }
            _boxes = new Box[4]
            {
                new Box{ Color = "White", Name = "Header" },
                new Box{ Color = "White", Name = "Body" },
                new Box{ Color = "White", Name = "Toolbar" },
                new Box{ Color = "White", Name = "Footer" }
            };
        }
        public Box GetBox(int boxNumber) => _boxes[boxNumber];
        public string GetBoxColor(int boxNumber) => _boxes[boxNumber].Color;
        public string GetBoxName(int boxNumber) => _boxes[boxNumber].Name;

        public IEnumerable<ContentItem> GetContentItems(int boxNumber)
        {
            return _items[boxNumber];
        }

        // Add a new todo item to a specific list
        public void AddTodo(int listNumber, string text, int boxNumber)
        {
            var newTodo = new ContentItem { Text = text, FontSize = 16, Box = boxNumber };
            _items[boxNumber].Add(newTodo);
        }

        // Remove a todo item from all lists
        public void RemoveTodo(ContentItem todo)
        {
            int boxNumber = todo.Box;
            _items[boxNumber].Remove(todo);
        }

        // Start dragging a todo item
        public void StartDrag(ContentItem todo)
        {
            draggedContentItem = todo;
        }

        // Drop the dragged todo item into the target list
        public void DropContentItem(int newBoxNumber)
        {
            if (draggedContentItem != null)
            {
                int oldBoxNumber = draggedContentItem.Box;
                _items[oldBoxNumber].Remove(draggedContentItem);
                _items[newBoxNumber].Add(draggedContentItem);
                draggedContentItem = null;
            }
        }

        // Save the background color for a specific box
        public void SaveBoxColor(int boxNumber, string color)
        {
            _boxes[boxNumber].Color = color;
        }
    }
}