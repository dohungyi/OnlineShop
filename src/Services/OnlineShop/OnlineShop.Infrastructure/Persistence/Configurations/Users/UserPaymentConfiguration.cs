using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Persistence.Configurations.Users;

public class UserPaymentConfiguration : IEntityTypeConfiguration<ApplicationUserPayment>
{
    public void Configure(EntityTypeBuilder<ApplicationUserPayment> builder)
    {
        builder.ToTable(TableName.UserPayment);
        
        builder.HasKey(up => up.Id);
    }
}