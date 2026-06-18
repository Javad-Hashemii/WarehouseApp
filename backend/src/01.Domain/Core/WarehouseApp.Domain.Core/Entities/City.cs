using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class City:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Workshop> Workshops { get; set; }
        public ICollection<Warehouse> Warehouses { get; set; }
    }
}
