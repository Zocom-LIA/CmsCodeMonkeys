using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Services
{
    public interface IContentItemService
    {
        ContentItemStorage ContentItemStorage { get; }

        void AddContentItem(int listNumber, string text);
        void DropContentItemItem(int targetListNumber);
        void RemoveContentItem(ContentItem ContentItem);
        void SaveBoxColor(int boxNumber, string color);
        void StartDrag(ContentItem contentItem);
    }
}