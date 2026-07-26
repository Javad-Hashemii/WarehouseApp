using Warehouse.Mvc.Models.Request;

namespace Warehouse.Mvc.Services
{
    public interface IRequestDraftService
    {
        RequestDraft GetDraft();

        void SaveDraft(RequestDraft draft);

        void AddItem(RequestDraftItem item);

        void RemoveItem(int productId);

        void Clear();
    }
}
