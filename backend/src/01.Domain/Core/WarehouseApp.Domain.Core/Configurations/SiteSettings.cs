using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Configurations
{
    public class SiteSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public CacheSettings CacheSettings { get; set; }
        public WarehouseConfigurations WarehouseConfigurations { get; set; }
        public FileSettings FileSettings { get; set; }
    }
}
