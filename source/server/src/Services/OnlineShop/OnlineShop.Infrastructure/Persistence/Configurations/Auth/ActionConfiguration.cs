using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;
using Action = OnlineShop.Domain.Entities.Action;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class ActionConfiguration : IEntityTypeConfiguration<Action>
{
    public void Configure(EntityTypeBuilder<Action> builder)
    {
        builder.ToTable(TableName.Action);
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Code).IsRequired().HasMaxLength(50).IsUnicode(false);

        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);

        builder.Property(a => a.Description).HasMaxLength(255);

        builder.Property(a => a.Exponent).IsRequired();
        
        builder.HasMany(a => a.RoleActions)
            .WithOne(ua => ua.Action)
            .HasForeignKey(ua => ua.ActionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}