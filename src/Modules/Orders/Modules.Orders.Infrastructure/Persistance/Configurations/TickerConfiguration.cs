using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Orders.Domain.Tickers;
using Modules.Orders.Domain.Tickers.ValueObjects;

namespace Modules.Orders.Infrastructure.Persistance.Configurations;

public sealed class TickerConfiguration : IEntityTypeConfiguration<Ticker>
{
    public void Configure(EntityTypeBuilder<Ticker> builder)
    {
        builder.ToTable("tickers");

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TickerId.Create(value)
            )
            .HasColumnName("id");

        builder
           .Property(x => x.Code)
           .HasColumnName("code")
           .HasMaxLength(10)
           .IsRequired();

        builder
           .Property(x => x.LastPrice)
           .HasColumnName("last_price")
           .HasPrecision(18, 2)
           .IsRequired();

        builder
           .Property(x => x.UpdatedOn)
           .HasColumnName("updated_on")
           .IsRequired();
    }
}
