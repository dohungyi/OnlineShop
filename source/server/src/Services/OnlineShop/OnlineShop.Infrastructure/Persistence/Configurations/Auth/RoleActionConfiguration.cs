using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;


namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class RoleActionConfiguration : IEntityTypeConfiguration<RoleAction>
{
    public void Configure(EntityTypeBuilder<RoleAction> builder)
    {
        builder.ToTable(TableName.RoleAction);
        
        builder.HasKey(ur => new { ur.ActionId, ur.RoleId });

        builder.HasOne(ur => ur.Role)
            .WithMany(u => u.RoleActions)
            .HasForeignKey(ur => ur.RoleId);

        builder.HasOne(ur => ur.Action)
            .WithMany(r => r.RoleActions)
            .HasForeignKey(ur => ur.ActionId);
    }
}