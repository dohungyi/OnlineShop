using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class UserConfigConfiguration : IEntityTypeConfiguration<ApplicationUserConfig>
{
    public void Configure(EntityTypeBuilder<ApplicationUserConfig> builder)
    {
        builder.ToTable(TableName.UserConfig);
    }
}