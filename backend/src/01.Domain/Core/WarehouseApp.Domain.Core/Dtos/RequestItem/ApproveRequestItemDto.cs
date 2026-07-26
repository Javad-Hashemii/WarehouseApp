using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.RequestItem
{
    public class ApproveRequestItemDto
    {
        public int RequestItemId { get; set; }
        public decimal QuantityApproved { get; set; }

    }
}
