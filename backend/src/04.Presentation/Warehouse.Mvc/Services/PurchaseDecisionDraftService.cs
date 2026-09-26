using System.Text.Json;
using Warehouse.Mvc.Models.Purchase;

namespace Warehouse.Mvc.Services
{
    public class PurchaseDecisionDraftService(IHttpContextAccessor httpContextAccessor):IPurchaseDecisionDraftService
    {
        private ISession Session =>
          httpContextAccessor.HttpContext!.Session;

        private static string GetSessionKey(int requestId)
            => $"PurchaseDecisionDraft_{requestId}";

        public PurchaseDecisionDraft? GetDraft(int requestId)
        {
            var json = Session.GetString(GetSessionKey(requestId));

            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            var draft = JsonSerializer.Deserialize<PurchaseDecisionDraft>(json);

            if (draft == null)
            {
                return null;
            }

            draft.Items ??= new List<PurchaseDecisionDraftItem>();

            return draft;
        }

        public void SaveDraft(PurchaseDecisionDraft draft)
        {
            var json = JsonSerializer.Serialize(draft);

            Session.SetString(GetSessionKey(draft.RequestId), json);
        }

        public void AddItem(int requestId, PurchaseDecisionDraftItem item)
        {
            var draft = GetDraft(requestId);

            if (draft == null)
            {
                return;
            }

            if (draft.Items.Any(i => i.ProductId == item.ProductId))
            {
                return;
            }

            draft.Items.Add(item);

            SaveDraft(draft);
        }

        public void RemoveItem(int requestId, int productId)
        {
            var draft = GetDraft(requestId);

            if (draft == null)
            {
                return;
            }

            var item = draft.Items
                .FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                return;
            }

            draft.Items.Remove(item);

            SaveDraft(draft);
        }

        public void Clear(int requestId)
        {
            Session.Remove(GetSessionKey(requestId));
        }
    }
}
