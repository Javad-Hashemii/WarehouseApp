namespace Warehouse.Mvc.Models.Request
{
    public class CreateRequestDraft
    {
        public string RequestNumber { get; set; } = string.Empty;

        public int WorkshopId { get; set; }

        public List<CreateRequestDraftItem> Items { get; set; } = new();
    }
}
