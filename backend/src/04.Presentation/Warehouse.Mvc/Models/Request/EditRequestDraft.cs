namespace Warehouse.Mvc.Models.Request
{
    public class EditRequestDraft
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; } = string.Empty;
        public int WorkshopId { get; set; }
        public List<EditRequestDraftItem> Items { get; set; } = new();
        public List<int> DeletedItemsIds { get; set; } = new();

    }
}
