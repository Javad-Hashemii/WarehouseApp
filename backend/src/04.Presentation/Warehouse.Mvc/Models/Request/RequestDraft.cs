namespace Warehouse.Mvc.Models.Request
{
    public class RequestDraft
    {
        public string RequestNumber { get; set; } = string.Empty;

        public int WorkshopId { get; set; }

        public List<RequestDraftItem> Items { get; set; } = new();
    }
}
