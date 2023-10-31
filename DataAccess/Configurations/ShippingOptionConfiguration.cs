using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Models;

namespace DataAccess.Configurations
{
    public class ShippingOptionConfiguration : IEntityTypeConfiguration<ShippingOption>
    {
        public void Configure(EntityTypeBuilder<ShippingOption> builder)
        {
            builder.HasData(
                new ShippingOption
                {
                    Id = new Guid("48ea541f-3ae3-4c10-9e52-6d43a2a33c5b"),
                    Method = "Standard Shipping",
                    DeliveryTime = "3-5 business days",
                    Cost = new decimal(5.99),
                    CreatedAt = new DateTime(2023, 10, 24, 17, 22, 10, 623, DateTimeKind.Local).AddTicks(8786),

                }
                , new ShippingOption
                {
                    Id = new Guid("47d3d393-b3aa-43a3-8f06-64387ba6bb8d"),
                    Method = "Expedited Shipping",
                    DeliveryTime = "2-3 business days",
                    Cost = new decimal(9.99),
                    CreatedAt = new DateTime(2023, 10, 24, 17, 22, 10, 623, DateTimeKind.Local).AddTicks(8786),
                }
                , new ShippingOption
                {
                    Id = new Guid("60046871-ba3e-4a2b-b02a-221075b3f9e4"),
                    Method = "Overnight Shipping",
                    DeliveryTime = "1 business day",
                    Cost = new decimal(19.99),
                    CreatedAt = new DateTime(2023, 10, 24, 17, 22, 10, 623, DateTimeKind.Local).AddTicks(8786)
                }
                );
        }
    }
}