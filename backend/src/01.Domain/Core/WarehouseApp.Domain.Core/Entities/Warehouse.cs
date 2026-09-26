using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class Warehouse:BaseEntity
    {

        public string Name { get; set; } = null!;

        public int CityId { get; set; }
        public City City { get; set; } = null!;

        public int? WorkshopId { get; set; }
        public Workshop? Workshop { get; set; }

        public StockLocation StockLocation { get; set; } = null!;

        public ICollection<Request> Requests { get; set; }
            = new List<Request>();

    }
}
