using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using Microsoft.EntityFrameworkCore;

namespace WarehouseApp.Infra.Data.Repos.Ef.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> query,int page,int pageSize,CancellationToken ct)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 20);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PaginatedResult<T>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
