using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Persistence.Configurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // TableName
        builder.ToTable(TableName.User);

        // Key
        builder.HasKey(u => u.Id);

        // Index
        builder.HasIndex(u => u.Id);
        
        builder.HasIndex(u => u.Username).IsUnique().IsClustered();

        // Property
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);

        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);

        builder.Property(u => u.Salt).IsRequired().HasMaxLength(255);

        builder.Property(u => u.PhoneNumber).HasMaxLength(20).IsUnicode(false);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(255).IsUnicode(false);

        builder.Property(u => u.FirstName).HasMaxLength(50);

        builder.Property(u => u.LastName).HasMaxLength(50);

        builder.Property(u => u.Address).HasMaxLength(255);

        builder.Property(u => u.Gender).IsRequired();

        builder.Property(u => u.ImageFileName).HasMaxLength(255);
        
        // Reference Property
        builder.HasOne(u => u.UserConfig)
            .WithOne(uc => (ApplicationUser)uc.User)
            .HasForeignKey<User>(u => u.UserConfigId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Ratings)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}