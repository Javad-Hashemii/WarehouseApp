using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseApp.Domain.Core.Dtos.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
