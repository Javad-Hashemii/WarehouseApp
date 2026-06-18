using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Configurations
{
    public class PaginationSettings
    {
        public int DefaultPageSize { get; set; }
        public int MaxPageSize { get; set; }
        public int ServicePageSizes { get; set; }
        public int ReviewPageSize { get; set; }
        public int SuggestionPageSize { get; set; }
        public int RequestsPageSize { get; set; }
    }
}
