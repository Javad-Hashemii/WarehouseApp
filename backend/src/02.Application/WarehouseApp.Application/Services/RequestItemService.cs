using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.RequestItem;
using WarehouseApp.Infra.Data.Repos.Ef.Repositories;

namespace WarehouseApp.Application.Services
{
    public class RequestItemService(RequestItemRepository requestItemRepository) : IRequestItemService
    {
        public async Task<ResultDto<bool>> CreateRequestItemAsync(CreateRequestItemsDto createRequestItemsDto, CancellationToken ct)
        {
            var result = await requestItemRepository.CreateRequestItemAsync(createRequestItemsDto, ct);
            if (result)
            {
                return ResultDto<bool>.Success("ایتم درخواست با موفقیت اضافه شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در ایجاد ایتم درخواست", false);
            }
            
        }

        public async Task<List<GetAllRequestItems>> GetAllRequestItemsAsync(CancellationToken ct)
        {
            return await requestItemRepository.GetAllRequestItemsAsync(ct);
        }

        public async Task<List<GetAllRequestItems>> GetAllRequestItemsByRequestIdAsync(int requestId, CancellationToken ct)
        {
            return await requestItemRepository.GetAllRequestItemsByRequestIdAsync(requestId, ct);
        }
    }
}
