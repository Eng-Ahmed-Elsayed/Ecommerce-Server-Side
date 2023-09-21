using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "e7590d23-edfe-4f3c-8677-39cb4c29b925",
                    Name = "Viewer",
                    NormalizedName = "VIEWER",
                },
                new IdentityRole
                {
                    Id = "9621e5b7-aae2-414c-ad07-6f8e04f56785",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
                );
        }
    }
}
