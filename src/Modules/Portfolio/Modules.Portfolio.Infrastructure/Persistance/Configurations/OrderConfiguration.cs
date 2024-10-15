using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Portfolio.Domain.Orders;
using Modules.Portfolio.Domain.Orders.ValueObjects;
using Modules.Portfolio.Domain.Tickers;
using Modules.Portfolio.Domain.Tickers.ValueObjects;

namespace Modules.Portfolio.Infrastructure.Persistance.Configurations;
public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(t => t.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value));

        builder
            .Property(x => x.TickerId)
            .HasColumnName("ticker_id")
            .ValueGeneratedNever()
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => TickerId.Create(value));

        builder
            .HasOne<Ticker>()
            .WithMany()
            .HasForeignKey(x => x.TickerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
           .Property(x => x.Quantity)
           .HasColumnName("quantity")
           .IsRequired();

        builder
            .Property(x => x.Price)
            .HasColumnName("price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .Property(x => x.Side)
            .HasColumnName("side")
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id")
            .ValueGeneratedNever()
            .IsRequired();
    }
}
