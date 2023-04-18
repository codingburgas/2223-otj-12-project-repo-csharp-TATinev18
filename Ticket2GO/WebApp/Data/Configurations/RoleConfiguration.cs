using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {

            builder.HasData(
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                 new IdentityRole
                 {
                     Name = "Company Manager",
                     NormalizedName = "COMPANY MANAGER"
                 },
                 new IdentityRole
                 {
                     Name = "User",
                     NormalizedName = "USER"
                 },
                 new IdentityRole
                 {
                     Name = "Unassigned",
                     NormalizedName = "UNASSIGNED"
                 }
                );
        }
    }
}

