using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class UserConfigConfiguration : BaseEntityConfiguration<ApplicationUserConfig>
{
    public void Configure(EntityTypeBuilder<ApplicationUserConfig> builder)
    {
        builder.ToTable(TableName.UserConfig);
    }
}