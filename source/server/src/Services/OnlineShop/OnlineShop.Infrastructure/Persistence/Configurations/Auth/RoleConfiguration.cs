using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;


namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(TableName.Role);
        
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Code).IsClustered();
            
        builder.Property(r => r.Code).IsRequired().HasMaxLength(50);

        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
    }
}