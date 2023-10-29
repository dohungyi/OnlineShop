using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class UserPaymentConfiguration : IEntityTypeConfiguration<ApplicationUserPayment>
{
    public void Configure(EntityTypeBuilder<ApplicationUserPayment> builder)
    {
        builder.ToTable(TableName.UserPayment);
        
        builder.HasKey(up => up.Id);
    }
}