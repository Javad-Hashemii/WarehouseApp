
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;

namespace HomeService.Domain.ApplicationServices._common.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedResult<T>(
            this IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 8;
            }

            if (pageSize > 20)
            {
                pageSize = 20;
            }
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedResult<T>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }
    }

}

