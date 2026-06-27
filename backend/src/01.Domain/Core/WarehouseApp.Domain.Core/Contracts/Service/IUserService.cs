using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.User;

namespace WarehouseApp.Domain.Core.Contracts.Service
{
    public interface IUserService
    {

        Task<ResultDto<bool>> LoginAsync(LoginUserDto userDto, CancellationToken cancellationToken);

        Task<ResultDto<bool>> RegisterAsync(RegisterUserDto userDto, CancellationToken cancellationToken);

        Task<ResultDto<bool>> CreateUserByAdminAsync(RegisterUserDto userDto, CancellationToken cancellationToken);

        Task<GetProfileDto> GetProfileAsync(int id, CancellationToken cancellationToken);

        Task<ResultDto<bool>> UpdateProfileAsync(
            int appUserId,
            GetProfileDto userDto,
            CancellationToken cancellationToken);

        Task<ResultDto<bool>> DeleteAsync(
            int appUserId,
            CancellationToken cancellationToken);

        Task<PaginatedResult<GetUserDto>> GetAllAsync(
            int page,
            CancellationToken cancellationToken);

        Task LogoutAsync();

        Task<ResultDto<bool>> ChangePasswordAsync(int userId,UpdateUserPasswordDto passwordDto,CancellationToken cancellationToken);
    }
}
