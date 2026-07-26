using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Warehouse
{
    public class UpdateWarehouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
    }
}
