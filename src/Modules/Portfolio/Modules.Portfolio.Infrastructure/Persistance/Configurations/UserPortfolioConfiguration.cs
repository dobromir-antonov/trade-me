using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Portfolio.Domain.UserPortfolios;

namespace Modules.Portfolio.Infrastructure.Persistance.Configurations;

//public sealed class UserPortfolioConfiguration : IEntityTypeConfiguration<UserPortfolio>
//{
//    public void Configure(EntityTypeBuilder<UserPortfolio> builder)
//    {
//        builder
//            .Property(x => x.Id)
//            .ValueGeneratedNever()
//            .HasConversion(
//                id => id.Value,
//                value => ClientId.Create(value)
//            )
//            .HasColumnName("Id");

//        builder
//            .Property(x => x.Name)
//            .HasColumnName("Name");
//    }
//}
