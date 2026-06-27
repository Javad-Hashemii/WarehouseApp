using HomeService.Domain.ApplicationServices._common.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WarehouseApp.Domain.Core.Configurations;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos._common;
using WarehouseApp.Domain.Core.Dtos.User;
using WarehouseApp.Domain.Core.Entities;

namespace WarehouseApp.Application.Services
{
    public class UserService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,IFileService fileService,RoleManager<IdentityRole<int>>roleManager  ,SiteSettings siteSettings) :  IUserService
    {
        public async Task<ResultDto<bool>> LoginAsync(LoginUserDto userDto, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByNameAsync(userDto.Username);
            if (user == null)
                return ResultDto<bool>.Failure("نام کاربری یا رمز عبور اشتباه است!");

            if (!await userManager.CheckPasswordAsync(user, userDto.Password))
                return ResultDto<bool>.Failure("نام کاربری یا رمز عبور اشتباه است!");


            if (!user.IsActive)
                return ResultDto<bool>.Failure("حساب کاربری شما غیرفعال است. لطفا با پشتیبانی تماس بگیرید.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
            };

            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContextAccessor.HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = userDto.RememberMe }
            );

            httpContextAccessor.HttpContext.User = principal;

            return ResultDto<bool>.Success("ورود با‌موفقیت انجام شد.");

        }


        private async Task<ResultDto<bool>> CreateUserAsync(RegisterUserDto userDto, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Initial Validations
                if (userDto.Password != userDto.ConfirmPassword)
                    return ResultDto<bool>.Failure("رمز عبور و تکرار آن باهم تطابق ندارند!");

                var existedUser = await userManager.Users.IgnoreQueryFilters()
                                                 .FirstOrDefaultAsync(u => u.UserName == userDto.Email, cancellationToken);

                if (existedUser != null)
                {
                    if (!existedUser.IsActive)
                        return ResultDto<bool>.Failure("این حساب توسط مدیر غیرفعال شده و امکان ثبت‌نام مجدد با این ایمیل وجود ندارد.");

                    return ResultDto<bool>.Failure("نام کاربری موجود است!");
                }

                var roleExists = await roleManager.RoleExistsAsync(userDto.Role);
                if (!roleExists)
                    return ResultDto<bool>.Failure("نقش انتخابی معتبر نیست!");

                // 2. Create YOUR Lean Identity User
                var user = new ApplicationUser()
                {
                    UserName = userDto.Email.Trim(),
                    Email = userDto.Email.Trim(),
                    FullName = userDto.FullName, // Assuming your DTO has this!
                    IsActive = false              // Activate the user by default upon registration
                };

                // 3. Save User to Database
                var result = await userManager.CreateAsync(user, userDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return ResultDto<bool>.Failure($"خطا در ثبت نام کاربر: {errors}");
                }

                // =====================================================================
                // DANGER ZONE: User is in the DB. If Role assignment fails, rollback!
                // =====================================================================

                // 4. Assign the Identity Role 
                var roleResult = await userManager.AddToRoleAsync(user, userDto.Role);
                if (!roleResult.Succeeded)
                {
                    // Rollback: Delete the orphaned user account
                    await userManager.DeleteAsync(user);

                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return ResultDto<bool>.Failure($"خطا در ثبت نقش: {errors}");
                }

                return ResultDto<bool>.Success("ثبت‌نام با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here in your production app
                return ResultDto<bool>.Failure("مشکلی پیش آمده در ثبت نام کاربر");
            }
        }
        public async Task<ResultDto<bool>> RegisterAsync(RegisterUserDto userDto, CancellationToken cancellationToken)
        {
            //idk why we have two methods for creating user but whatever, maybe in the future we will add some extra logic for admin creating user

            return await CreateUserAsync(userDto, cancellationToken);
        }
        public async Task<ResultDto<bool>> CreateUserByAdminAsync(RegisterUserDto userDto, CancellationToken cancellationToken)
        {

            return await CreateUserAsync(userDto, cancellationToken);
        }
        public async Task<GetProfileDto> GetProfileAsync(int id, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            var roles = await userManager.GetRolesAsync(user);

            var profile = new GetProfileDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Role = roles.FirstOrDefault(),
            };

            return profile;
        }
        public async Task<ResultDto<bool>> UpdateProfileAsync(int appUserId, GetProfileDto userDto, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByIdAsync(appUserId.ToString());

            user.FullName = userDto.FullName;


            if (!string.IsNullOrWhiteSpace(userDto.NewPassword))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var resetResult = await userManager.ResetPasswordAsync(user, token, userDto.NewPassword);

                if (!resetResult.Succeeded)
                {
                    var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                    return ResultDto<bool>.Failure($"تغییر رمز عبور انجام نشد: {errors}");
                }
            }

            var identityResult = await userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                return ResultDto<bool>.Failure($"مشکلی در ویرایش رخ داد! : {errors}");
            }

            var roles = await userManager.GetRolesAsync(user);
            bool updateResult = false;


            updateResult = identityResult.Succeeded;
            

            if (updateResult == true || identityResult.Succeeded)
            {
                return ResultDto<bool>.Success("کاربر با موفقیت ویرایش شد.");
            }

            return ResultDto<bool>.Failure("ویرایش اطلاعات انجام نشد!");
        }
        public async Task<ResultDto<bool>> DeleteAsync(int appUserId, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(appUserId.ToString());
            if (user == null)
                return ResultDto<bool>.Failure("کاربر یافت نشد");


            user.IsActive = false;
            user.DeletedAt = DateTime.Now;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResultDto<bool>.Failure($"خطا در حذف کاربر: {errors}");
            }

            return ResultDto<bool>.Success("کاربر با موفقیت حذف شد!");
        }
        public async Task<PaginatedResult<GetUserDto>> GetAllAsync(int page, CancellationToken cancellationToken)
        {
            var query = userManager.Users
               .OrderByDescending(u => u.Id)
               .Select(u => new GetUserDto
               {
                   Id = u.Id,
                   Email = u.Email,
               });

            var pageSize = siteSettings.WarehouseConfigurations.PaginationSettings.DefaultPageSize;
            var paged = await query.ToPaginatedResult(page, pageSize, cancellationToken);

            foreach (var item in paged.Items)
            {
                var user = await userManager.FindByIdAsync(item.Id.ToString());
                var roles = await userManager.GetRolesAsync(user);
                item.Role = string.Join(", ", roles);
            }

            return paged;
        }
        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }
        public async Task<ResultDto<bool>> ChangePasswordAsync(int userId, UpdateUserPasswordDto passwordDto, CancellationToken cancellationToken)
        {
            if (passwordDto.ConfirmPassword != passwordDto.NewPassword)
            {
                return ResultDto<bool>.Failure("رمز عبور و تکرار آن باهم تطابق ندارند!");
            }

            var user = await userManager.FindByIdAsync(userId.ToString());

            var result = await userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

            if (result.Succeeded == false)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResultDto<bool>.Failure($"رمز عبور ویرایش نشد: {errors}");
            }

            await signInManager.RefreshSignInAsync(user);
            return ResultDto<bool>.Success("پسورد با موفقیت ویرایش شد.");
        }

        public async Task CreateRoleAsync(string roleName)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                // Instantiate IdentityRole<int> instead of IdentityRole
                var roleResult = await roleManager.CreateAsync(new IdentityRole<int>(roleName));

                if (!roleResult.Succeeded)
                {
                    // Handle exceptions or map to a domain-specific result/exception
                    throw new Exception($"Failed to create role: {roleName}");
                }
            }
        }

        public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null) return false;

            // Check if the user already has the role to prevent duplicate assignment errors
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }

            return true; // User already had the role
        }

    }
}
