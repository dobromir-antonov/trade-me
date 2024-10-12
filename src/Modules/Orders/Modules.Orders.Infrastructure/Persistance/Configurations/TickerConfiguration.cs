using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Price.Domain;
using Modules.Price.Domain.Tickers;

namespace Modules.Portfolio.Infrastructure.Persistance.Configurations;

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
            .HasColumnName("Id");

        builder
           .Property(x => x.Code)
           .HasColumnName("Code")
           .HasMaxLength(10)
           .IsRequired();

        builder
            .Property(x => x.Name)
            .HasColumnName("Name")
            .HasMaxLength(50)
            .IsRequired();

        builder
           .Property(x => x.LastPrice)
           .HasColumnName("LastPrice")
           .HasPrecision(18, 2)
           .IsRequired();

        builder
           .Property(x => x.UpdatedOn)
           .HasColumnName("UpdatedOn")
           .IsRequired();
    }
}
