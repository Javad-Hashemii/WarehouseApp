using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        int? WorkshopId { get; }
        string? Role { get; }
    }
}
