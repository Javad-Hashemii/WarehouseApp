using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.User
{
    public class GetProfileDto
    {

        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
        public IFormFile? ProfileFile { get; set; }
        public string? Role { get; set; }
        public DateTime? CreatedAt { get; set; } //تاریخ عضویت در سایت
    }
}
