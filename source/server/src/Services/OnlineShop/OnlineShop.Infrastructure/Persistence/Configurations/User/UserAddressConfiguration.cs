using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class UserAddressConfiguration : IEntityTypeConfiguration<ApplicationUserAddress>
{
    public void Configure(EntityTypeBuilder<ApplicationUserAddress> builder)
    {
        builder.ToTable(TableName.UserAddress);
        
        builder.HasKey(ua => ua.Id);
    }
}