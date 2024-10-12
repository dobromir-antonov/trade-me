using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Orders.Domain;
using Modules.Orders.Domain.ValueObjects;

namespace Modules.Portfolio.Infrastructure.Persistance.Configurations;
public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(t => t.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value)
            )
            .HasColumnName("Id");

        builder
            .Property(x => x.Quantity)
            .HasColumnName("Quantity")
            .IsRequired();

        builder
            .Property(x => x.Price)
            .HasColumnName("Price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .HasColumnName("UserId")
            .ValueGeneratedNever()
            .IsRequired();
    }
}
