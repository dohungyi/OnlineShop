using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class SignInHistoryConfiguration : BaseEntityConfiguration<SignInHistory>
{
    public void Configure(EntityTypeBuilder<SignInHistory> builder)
    {
        builder.ToTable(TableName.SignInHistory);

        builder.HasKey(s => s.Id);
    }
}