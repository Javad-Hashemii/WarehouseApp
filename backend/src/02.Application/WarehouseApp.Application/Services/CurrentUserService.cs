using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using WarehouseApp.Domain.Core.Contracts.Service;

namespace WarehouseApp.Application.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private ClaimsPrincipal? User =>
         httpContextAccessor.HttpContext?.User;


        public int UserId
        {
            get
            {
                var idClaim = User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(idClaim, out var id))
                {
                    throw new UnauthorizedAccessException();
                }

                return id;
            }
        }

        public int? WorkshopId
        {
            get
            {
                var workshopIdClaim = User?.FindFirstValue("WorkshopId");

                return int.TryParse(workshopIdClaim, out var workshopId)
                    ? workshopId
                    : null;
            }
        }
        public string? Role => User?.FindFirstValue(ClaimTypes.Role);
    }
}
