using System.Text.Json;
using Warehouse.Mvc.Models.Request;

namespace Warehouse.Mvc.Services
{
    public class RequestDraftService(IHttpContextAccessor httpContextAccessor) : IRequestDraftService
    {
        private const string SessionKey = "RequestDraft";

        private ISession Session =>
        httpContextAccessor.HttpContext!.Session;

        public void AddItem(RequestDraftItem item)
        {
            var draft = GetDraft();

            draft.Items.Add(item);

            SaveDraft(draft);
        }

        public void Clear()
        {
            Session.Remove(SessionKey);
        }

        public RequestDraft GetDraft()
        {
            var json = Session.GetString(SessionKey);

            if (json == null)
            {
                return new RequestDraft();
            }

            return JsonSerializer.Deserialize<RequestDraft>(json)
                   ?? new RequestDraft();
        }

        public void RemoveItem(int productId)
        {
            var draft = GetDraft();

            var item = draft.Items
                .FirstOrDefault(x => x.ProductId == productId);

            if (item != null)
            {
                draft.Items.Remove(item);
            }

            SaveDraft(draft);
        }

        public void SaveDraft(RequestDraft draft)
        {
            var json = JsonSerializer.Serialize(draft);

            Session.SetString(SessionKey, json);
        }
    }
}
