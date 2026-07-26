using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.RequestHistory;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Application.Services
{
    public class RequestHistoryService(IRequestHistoryRepository requestHistoryRepository) : IRequestHistoryService
    {
        public async void CreateRequestHistoryAsync(CreateRequestHistoryDto createRequestHistoryDto) //void???
        {


            var result =  requestHistoryRepository.CreateRequestHistory(createRequestHistoryDto);
            //if (result)
            //{
            //    return ResultDto<bool>.Success("Request history created successfully."); //should it return a message?
            //}
            //else
            //{
            //    return ResultDto<bool>.Failure("Failed to create request history."); //should it return a message?
            //}
        }

        public async Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesAsync(CancellationToken ct)
        {
            return await requestHistoryRepository.GetAllRequestHistoriesAsync(ct);
        }

        public async Task<List<GetAllRequestHistoriesDto>> GetAllRequestHistoriesByRequestIdAsync(int requestId,CancellationToken ct)
        {
            return await requestHistoryRepository.GetAllRequestHistoriesByRequestIdAsync(requestId, ct);
        }
    }
    
}
