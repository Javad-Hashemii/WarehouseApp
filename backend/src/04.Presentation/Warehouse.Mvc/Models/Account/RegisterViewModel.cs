using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Mvc.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "لطفا نام و نام خانوادگی را وارد کنید")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل معتبر نیست")]
        [Display(Name = "ایمیل (نام کاربری)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن تطابق ندارند")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "لطفا نقش خود را انتخاب کنید")]
        [Display(Name = "نوع حساب کاربری")]
        public string SelectedRole { get; set; }

        public IEnumerable<SelectListItem> AvailableRoles { get; set; } = new List<SelectListItem>();
    }
}
