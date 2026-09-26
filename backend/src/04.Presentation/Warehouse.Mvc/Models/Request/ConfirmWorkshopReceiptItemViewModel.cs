namespace Warehouse.Mvc.Models.Request
{
    public class ConfirmWorkshopReceiptItemViewModel
    {
        public int RequestItemId { get; set; }
        public decimal QuantityReceived { get; set; }
    }
}
