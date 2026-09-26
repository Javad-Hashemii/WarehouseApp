using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class ApplicationUser:IdentityUser<int>
    {
        public string FullName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int? WorkshopId { get; set; }
        public Workshop? Workshop { get; set; }

        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public ICollection<RequestHistory> RequestHistories { get; set; }= new List<RequestHistory>();
        public ICollection<PurchaseRequestHistory> PurchaseRequestHistories { get; set; } = new List<PurchaseRequestHistory>();

    }
}
