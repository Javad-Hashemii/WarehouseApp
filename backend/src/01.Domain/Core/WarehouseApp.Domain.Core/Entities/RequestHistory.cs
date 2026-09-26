using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Domain.Core.Entities
{
    public class RequestHistory:HistoryBase
    {

        public int RequestId { get; set; }
        public RequestStatus FromStatus { get; set; }
        public RequestStatus ToStatus { get; set; }


        public Request Request { get; set; } = null!;
    }
}
