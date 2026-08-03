using Warehouse.Mvc.Models.Request;

namespace Warehouse.Mvc.Services
{
    public interface IEditRequestDraftService
    {
        EditRequestDraft? GetEditRequestDraft(int requestId);
        void SaveDraft(EditRequestDraft draft);

        void AddItem(int requestId, EditRequestDraftItem item);

        void RemoveItem(int requestId, int productId);

        void Clear(int requestId);
    }
}
