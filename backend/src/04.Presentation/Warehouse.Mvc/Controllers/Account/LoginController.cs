using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Warehouse.Mvc.Models.Account;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos.User;

namespace Warehouse.Mvc.Controllers.Account
{
    [Route("Account/Login")]
    public class LoginController(IUserService userService) : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel viewModel, CancellationToken cancellationToken, string returnUrl = null)
        {
            // 1. Check UI validation (ViewModel)
            if (!ModelState.IsValid)
            {
                return View("~/Views/Account/Login.cshtml", viewModel);
            }

            // 2. TURN VIEWMODEL INTO DTO
            var userDto = new LoginUserDto
            {
                Username = viewModel.Username,
                Password = viewModel.Password,
                RememberMe = viewModel.RememberMe
            };

            // 3. Pass the DTO to your exact service method
            var result = await userService.LoginAsync(userDto, cancellationToken);

            // 4. Handle the ResultDto<bool> from your service
            if (result.IsSuccess)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Dashboard");
            }

            // If it failed, show your Persian error message on the view
            ModelState.AddModelError(string.Empty, result.Message);

            return View("~/Views/Account/Login.cshtml", viewModel);
        }

        [HttpPost("/Account/Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Signs the user out of the exact scheme you used to sign them in
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // Send them back to the homepage after logging out
            return RedirectToAction("Index", "Home");
        }
    }
}
