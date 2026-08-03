namespace Warehouse.Mvc.Models.Request
{
    public class RequestDetailsViewModel
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
        public string WorkshopName { get; set; }
        public string RequestedBy { get; set; }
        public List<CreateRequestItemViewModel> Items { get; set; }
    }
}
