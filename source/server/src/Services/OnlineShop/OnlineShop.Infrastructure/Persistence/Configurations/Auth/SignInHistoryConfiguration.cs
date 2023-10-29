using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class SignInHistoryConfiguration : IEntityTypeConfiguration<SignInHistory>
{
    public void Configure(EntityTypeBuilder<SignInHistory> builder)
    {
        builder.ToTable(TableName.SignInHistory);

        builder.HasKey(s => s.Id);
    }
}