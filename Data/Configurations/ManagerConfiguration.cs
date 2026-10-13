using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Data.Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Phone).HasMaxLength(20).IsRequired();
        builder.Property(m => m.Email).HasMaxLength(254).IsRequired();
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("now()");
        builder.HasIndex(m => m.Email).IsUnique();
    }
}
