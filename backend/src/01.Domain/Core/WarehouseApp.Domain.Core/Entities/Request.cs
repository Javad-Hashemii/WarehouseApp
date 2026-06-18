using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Entities
{
    public class Request:BaseEntity //all entities have Id and we have base entity without id in it wtf XXXX Change the fluent api
    {

        public string RequestNumber { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public int RequestedByUserId { get; set; }
        public int WorkshopId { get; set; }
        public int? WarehouseId { get; set; }
        public RequestStatus Status { get; set; }
        public string? Notes { get; set; }


        // Navigation properties
        public ApplicationUser RequestedByUser { get; set; } = null!;
        public Workshop Workshop { get; set; } = null!;
        public Warehouse? Warehouse { get; set; }
        public ICollection<RequestItem> RequestItems { get; set; } = new List<RequestItem>();
        public ICollection<RequestHistory> History { get; set; } = new List<RequestHistory>();
    }
}
