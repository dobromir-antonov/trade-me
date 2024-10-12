using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Orders.Domain;
using Modules.Orders.Domain.ValueObjects;

namespace Modules.Portfolio.Infrastructure.Persistance.Configurations;

/// <summary>
/// TODO: Add database context configuration, example: Postegres, MSSQL, etc.
/// </summary>
public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasColumnName("Id")
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value)
            );

        builder
            .Property(x => x.Quantity)
            .HasColumnName("Quantity")
            .IsRequired();

        builder
            .Property(x => x.Price)
            .HasColumnName("Price")
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .HasColumnName("UserId")
            .ValueGeneratedNever()
            .IsRequired();
    }
}
