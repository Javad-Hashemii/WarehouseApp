using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class Workshop:BaseEntity
    {
        public string Name { get; set; } = null!;

        public int CityId { get; set; }
        public City City { get; set; } = null!;

        public Warehouse? Warehouse { get; set; }

        public StockLocation? StockLocation { get; set; }

        public ICollection<Request> Requests { get; set; }
            = new List<Request>();

        public ICollection<ApplicationUser> WorkshopUsers { get; set; }
            = new List<ApplicationUser>();
    }
}
