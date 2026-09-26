using Warehouse.Mvc.Models.Purchase;

namespace Warehouse.Mvc.Services
{
    public interface IPurchaseDecisionDraftService
    {
        PurchaseDecisionDraft? GetDraft(int requestId);
        void SaveDraft(PurchaseDecisionDraft draft);
        void AddItem(int requestId, PurchaseDecisionDraftItem item);
        void RemoveItem(int requestId, int productId);
        void Clear(int requestId);
    }
}
