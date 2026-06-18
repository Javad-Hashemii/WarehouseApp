using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Configurations
{
    public class CacheSettings
    {
        public int AbsoluteExpirationHours { get; set; }
        public int SlidingExpirationHours { get; set; }
    }
}
