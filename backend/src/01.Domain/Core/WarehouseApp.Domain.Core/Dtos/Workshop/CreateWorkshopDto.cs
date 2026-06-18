using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.Workshop
{
    public class CreateWorkshopDto
    {
        public string Name { get; set; }
        public int CityId { get; set; }
    }
}
