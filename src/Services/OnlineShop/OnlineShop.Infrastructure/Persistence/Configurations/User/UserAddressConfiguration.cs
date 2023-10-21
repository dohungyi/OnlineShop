using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class UserAddressConfiguration : BaseEntityConfiguration<ApplicationUserAddress>
{
    public void Configure(EntityTypeBuilder<ApplicationUserAddress> builder)
    {
        builder.ToTable(TableName.UserAddress);
        
        builder.HasKey(ua => ua.Id);
    }
}