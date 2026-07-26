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
        public string? Role => User?.FindFirstValue(ClaimTypes.Role);
    }
}
