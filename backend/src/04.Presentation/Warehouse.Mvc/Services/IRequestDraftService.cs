using Warehouse.Mvc.Models.Request;

namespace Warehouse.Mvc.Services
{
    public interface IRequestDraftService
    {
        CreateRequestDraft GetDraft();

        void SaveDraft(CreateRequestDraft draft);

        void AddItem(CreateRequestDraftItem item);

        void RemoveItem(int productId);

        void Clear();
    }
}
