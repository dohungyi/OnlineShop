using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class RequestInformationConfiguration : IEntityTypeConfiguration<RequestInformation>
{
    public void Configure(EntityTypeBuilder<RequestInformation> builder)
    {
        builder.ToTable(TableName.RequestInformation);
        
        builder.HasKey(ri => ri.Id);
    }
}