using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain;

namespace SharedKernel.Persistence;

public class BaseEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TKey>
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.CreatedBy).IsRequired();
        builder.Property(e => e.LastModifiedDate);
        builder.Property(e => e.LastModifiedBy);
        builder.Property(e => e.DeletedDate);
        builder.Property(e => e.DeletedBy);
        builder.Ignore(e => e.DomainEvents); 
    }
}

public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.CreatedBy).IsRequired();
        builder.Property(e => e.LastModifiedDate);
        builder.Property(e => e.LastModifiedBy);
        builder.Property(e => e.DeletedDate);
        builder.Property(e => e.DeletedBy);
        builder.Ignore(e => e.DomainEvents); 
    }
}