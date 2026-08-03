using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WarehouseApp.Domain.Core.Consts;

namespace WarehouseApp.Infra.Db.SqlServer.Ef.Configurations
{
    public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            
            builder.HasData(
                new IdentityUserRole<int> { UserId = 67, RoleId = 1 }
            );
        }
    }
}
