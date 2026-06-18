using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Repository;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.Request;
using WarehouseApp.Domain.Core.Enums;

namespace WarehouseApp.Application.Services
{
    public class RequestService(IRequestRepository requestRepository) : IRequestService
    {
        public async Task<ResultDto<bool>> CreateRequestAsync(CreateRequestDto createRequestDto, CancellationToken ct)
        {
            var result = await requestRepository.CreateRequestAsync(createRequestDto, ct);
            if (result)
            {
                return ResultDto<bool>.Success("درخواست با موفقیت اضافه شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در ایجاد درخواست", false);
            }
        }

        public Task<bool> DeleteRequestAsync(int requestId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<GetRequestDto?> GetByIdWithDetailsAsync(int id,CancellationToken ct)
        {
            return await requestRepository.GetByIdWithDetailsAsync(id, ct);
        }

        public async Task<IEnumerable<GetAllRequestsDto>> GetSummariesByStatusAsync(RequestStatus status,CancellationToken ct)
        {
            return await requestRepository.GetSummariesByStatusAsync(status, ct);
        }
        

        public async Task<IEnumerable<GetAllRequestsDto>> GetSummariesByWarehouseAsync(int warehouseId,CancellationToken ct)
        {
            return await requestRepository.GetSummariesByWarehouseAsync(warehouseId, ct);
        }

        public async Task<List<GetAllRequestsDto>> GetSummariesByWorkshopAsync(int workshopId,CancellationToken ct)
        {
            return await requestRepository.GetSummariesByWorkshopAsync(workshopId, ct);
        }

        public async Task<ResultDto<bool>> UpdateRequestAsync(UpdateRequestStatusDto updateStatusDto, CancellationToken ct)
        {
            var result = await requestRepository.UpdateRequestAsync(updateStatusDto, ct);
            if (result)
            {
                return ResultDto<bool>.Success("درخواست با موفقیت به روزرسانی شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در به روزرسانی درخواست", false);
            }
        }

        public async Task<ResultDto<bool>> UpdateStatusAsync(UpdateRequestStatusDto updateStatusDto, CancellationToken ct)
        {
            var result = await requestRepository.UpdateStatusAsync(updateStatusDto, ct);
            if (result)
            {
                return ResultDto<bool>.Success("وضعیت درخواست با موفقیت به روزرسانی شد", true);
            }
            else
            {
                return ResultDto<bool>.Failure("خطا در به روزرسانی وضعیت درخواست", false);
            }

        }
    }
}
    

