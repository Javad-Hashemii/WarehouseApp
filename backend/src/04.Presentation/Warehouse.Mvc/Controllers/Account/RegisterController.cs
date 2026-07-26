using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warehouse.Mvc.Models.Account;
using WarehouseApp.Domain.Core.Contracts.Service;
using WarehouseApp.Domain.Core.Dtos.User;

namespace Warehouse.Mvc.Controllers.Account
{
    [Route("Account/Register")]
    public class RegisterController(IUserService _userService) : Controller
    {
        private IEnumerable<SelectListItem> GetSafeRoles()
        {
            return new List<SelectListItem>
    {
        new SelectListItem
        {
            Value = "WorkshopEmployee",
            Text = "کارمند کارگاه"
        },

        new SelectListItem
        {
            Value = "WorkshopManager",
            Text = "مدیر کارگاه"
        },

        new SelectListItem
        {
            Value = "WarehouseManager",
            Text = "مدیر انبار"
        },

        new SelectListItem
        {
            Value = "CentralManager",
            Text = "مدیر دفتر مرکزی"
        },

        new SelectListItem
        {
            Value = "ProcurementOfficer",
            Text = "مسئول تدارکات"
        }
    };
        }


        [HttpGet("")]
        public IActionResult Index()
        {
            var viewModel = new RegisterViewModel
            {
                // 1. Use the helper method to build the HTML dropdown
                AvailableRoles = GetSafeRoles()
            };
            return View("~/Views/Account/Register.cshtml", viewModel);
        }


        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AvailableRoles = GetSafeRoles();
                return View("~/Views/Account/Register.cshtml", viewModel);
            }

            // MAP EXACTLY TO YOUR DTO
            var userDto = new RegisterUserDto
            {
                FullName = viewModel.FullName,
                Email = viewModel.Email,
                Password = viewModel.Password,
                ConfirmPassword = viewModel.ConfirmPassword,
                Role = viewModel.SelectedRole
            };

            var result = await _userService.RegisterAsync(userDto, cancellationToken);
            if (result.IsSuccess)
            {
                // Hardcode the exact URL path. Bulletproof.
                return Redirect("/Account/RegistrationPending");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            viewModel.AvailableRoles = GetSafeRoles();
            return View("~/Views/Account/Register.cshtml", viewModel);
        }

        [HttpGet("/Account/RegistrationPending")]
        public IActionResult RegistrationPending()
        {
            // This tells it exactly which HTML file to load
            return View("~/Views/Account/RegistrationPending.cshtml");
        }
    }
}
