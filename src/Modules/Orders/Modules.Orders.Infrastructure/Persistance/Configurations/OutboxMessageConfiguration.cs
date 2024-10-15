using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Orders.Infrastructure.Outbox;

namespace Modules.Orders.Infrastructure.Persistance.Configurations;

public sealed class OutboxIntegrationEventConfiguration : IEntityTypeConfiguration<OutboxIntegrationEvent>
{
    public void Configure(EntityTypeBuilder<OutboxIntegrationEvent> builder)
    {
        builder.ToTable("outbox_integration_events");
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder
           .Property(x => x.Type)
           .HasColumnName("type")
           .HasColumnType("VARCHAR")
           .HasMaxLength(255)
           .IsRequired();

        builder
           .Property(x => x.Content)
           .HasColumnName("content")
           .HasColumnType("JSONB")
           .IsRequired();

        builder
          .Property(x => x.CreatedOnUtc)
          .HasColumnName("created_on_utc")
          .HasColumnType("TIMESTAMP WITH TIME ZONE")
          .IsRequired();

        builder
          .Property(x => x.ProcessedOnUtc)
          .HasColumnType("TIMESTAMP WITH TIME ZONE")
          .HasColumnName("processed_on_utc");

        builder
           .Property(x => x.Error)
           .HasColumnName("error");

        builder
            .HasIndex(x => new { x.CreatedOnUtc, x.ProcessedOnUtc })
            .IncludeProperties(x => new { x.Id, x.Type, x.Content });
    }
}
