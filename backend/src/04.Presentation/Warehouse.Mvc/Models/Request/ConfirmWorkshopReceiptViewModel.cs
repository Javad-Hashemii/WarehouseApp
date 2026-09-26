namespace Warehouse.Mvc.Models.Request
{
    public class ConfirmWorkshopReceiptViewModel
    {
        public int RequestId { get; set; }
        public List<ConfirmWorkshopReceiptItemViewModel> Items { get; set; } = new();
    }
}
