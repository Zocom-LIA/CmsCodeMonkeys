using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public class ContentItemService : IContentItemService
    {
        // ContentItem lists
        public ContentItemStorage ContentItemStorage => _storage;
        private ContentItemStorage _storage = new ContentItemStorage();
        private ContentItem? _draggedContentItem;

        // Add a new ContentItem item to a specific list
        public void AddContentItem(int listNumber, string text)
        {
            var newContentItem = new ContentItem { Text = text, FontSize = 16 };
            switch (listNumber)
            {
                case 1:
                    _storage.ContentItemList1.Add(newContentItem);
                    break;
                case 2:
                    _storage.ContentItemList2.Add(newContentItem);
                    break;
                case 3:
                    _storage.ContentItemList3.Add(newContentItem);
                    break;
                case 4:
                    _storage.ContentItemList4.Add(newContentItem);
                    break;
            }
        }

        // Remove a ContentItem item from all lists
        public void RemoveContentItem(ContentItem ContentItem)
        {
            _storage.ContentItemList1.Remove(ContentItem);
            _storage.ContentItemList2.Remove(ContentItem);
            _storage.ContentItemList3.Remove(ContentItem);
            _storage.ContentItemList4.Remove(ContentItem);
        }

        // Start dragging a ContentItem item
        public void StartDrag(ContentItem contentItem)
        {
            _draggedContentItem = contentItem;
        }

        // Drop the dragged ContentItem item into the target list
        public void DropContentItemItem(int targetListNumber)
        {
            if (_draggedContentItem != null)
            {
                _storage.ContentItemList1.Remove(_draggedContentItem);
                _storage.ContentItemList2.Remove(_draggedContentItem);
                _storage.ContentItemList3.Remove(_draggedContentItem);
                _storage.ContentItemList4.Remove(_draggedContentItem);

                switch (targetListNumber)
                {
                    case 1:
                        _storage.ContentItemList1.Add(_draggedContentItem);
                        break;
                    case 2:
                        _storage.ContentItemList2.Add(_draggedContentItem);
                        break;
                    case 3:
                        _storage.ContentItemList3.Add(_draggedContentItem);
                        break;
                    case 4:
                        _storage.ContentItemList4.Add(_draggedContentItem);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(targetListNumber));
                }
                _draggedContentItem = null;
            }
        }

        // Save the background color for a specific box
        public void SaveBoxColor(int boxNumber, string color)
        {
            switch (boxNumber)
            {
                case 1:
                    _storage.Box1Color = color;
                    break;
                case 2:
                    _storage.Box2Color = color;
                    break;
                case 3:
                    _storage.Box3Color = color;
                    break;
                case 4:
                    _storage.Box4Color = color;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(boxNumber));
            }
        }
    }
}