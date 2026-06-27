using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Entities
{
    public class Warehouse:BaseEntity
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public ICollection<InventoryBalance> InventoryBalances { get; set; }
        public ICollection<Transaction> OutgoingTransactions { get; set; }
        public ICollection<Transaction> IncomingTransactions { get; set; }
        public ICollection<Request> Requests { get; set; }
    }
}
