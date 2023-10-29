using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Query
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        // TableName
        builder.ToTable(TableName.User);

        // Key
        builder.HasKey(u => u.Id);

        // Non - Index Clustered   
        builder.HasIndex(u => u.Id);
        
        // Index Clustered 
        builder.HasIndex(u => u.Username).IsUnique().IsClustered();

        // Property
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);

        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);

        builder.Property(u => u.Salt).IsRequired().HasMaxLength(255);

        builder.Property(u => u.PhoneNumber).HasMaxLength(20).IsUnicode(false);

        builder.Property(u => u.Email).HasMaxLength(255).IsUnicode(false);

        builder.Property(u => u.FirstName).HasMaxLength(50);

        builder.Property(u => u.LastName).HasMaxLength(50);

        builder.Property(u => u.Address).HasMaxLength(255);
        
        // Reference Property
        builder.HasOne(u => u.Avatar)
            .WithOne(a => a.User)
            .HasForeignKey<Avatar>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        builder.HasOne(u => u.UserConfig)
            .WithOne(uc => uc.User)
            .HasForeignKey<ApplicationUserConfig>(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(u => u.RefreshToken)
            .WithOne(uc => uc.User)
            .HasForeignKey<RefreshToken>(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        builder.HasMany(u => u.SignInHistories)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        // builder.HasMany(u => u.UserAddresses)
        //     .WithOne(ur => ur.User)
        //     .HasForeignKey(ur => ur.UserId)
        //     .OnDelete(DeleteBehavior.Cascade)
        //     .IsRequired(false);
        //
        // builder.HasMany(u => u.UserPayments)
        //     .WithOne(ur => ur.User)
        //     .HasForeignKey(ur => ur.UserId)
        //     .OnDelete(DeleteBehavior.Cascade)
        //     .IsRequired(false);
        //
        // builder.HasMany(u => u.Ratings)
        //     .WithOne(r => r.User)
        //     .HasForeignKey(r => r.UserId)
        //     .OnDelete(DeleteBehavior.Cascade)
        //     .IsRequired(false);
        //
        // builder.HasMany(u => u.Orders)
        //     .WithOne(o => o.User)
        //     .HasForeignKey(o => o.UserId)
        //     .OnDelete(DeleteBehavior.Cascade)
        //     .IsRequired(false);

    }
}