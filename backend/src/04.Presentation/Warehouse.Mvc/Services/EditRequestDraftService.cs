using System.Text.Json;
using Warehouse.Mvc.Models.Request;

namespace Warehouse.Mvc.Services;

public class EditRequestDraftService(IHttpContextAccessor httpContextAccessor): IEditRequestDraftService
{
    private ISession Session =>
        httpContextAccessor.HttpContext!.Session;

    private static string GetSessionKey(int requestId)
        => $"EditRequestDraft_{requestId}";

    public EditRequestDraft GetEditRequestDraft(int requestId)
    {
        var json = Session.GetString(GetSessionKey(requestId));

        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        var draft = JsonSerializer.Deserialize<EditRequestDraft>(json);

        if (draft is null)
        {
            return null;
        }

        draft.Items ??= new List<EditRequestDraftItem>();
        draft.DeletedItemsIds ??= new List<int>();

        return draft;
    }

    public void SaveDraft(EditRequestDraft draft)
    {
        var json = JsonSerializer.Serialize(draft);

        Session.SetString(GetSessionKey(draft.RequestId),json);
    }

    public void AddItem(int requestId,EditRequestDraftItem item)
    {
        var draft = GetEditRequestDraft(requestId);

        draft.Items.Add(item);

        SaveDraft(draft);
    }

    public void RemoveItem(int requestId,int productId)
    {
        var draft = GetEditRequestDraft(requestId);

        var item = draft.Items
            .FirstOrDefault(x => x.ProductId == productId);

        if (item is null)
        {
            return;
        }

        if (item.RequestItemId.HasValue)
        {
            draft.DeletedItemsIds.Add(item.RequestItemId.Value);
        }

        draft.Items.Remove(item);

        SaveDraft(draft);
    }

    public void Clear(int requestId)
    {
        Session.Remove(GetSessionKey(requestId));
    }
}