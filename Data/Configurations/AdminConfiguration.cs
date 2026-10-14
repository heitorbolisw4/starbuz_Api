using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Data.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).HasMaxLength(Admin.NameMaxLength).IsRequired();
        builder.Property(a => a.Email).HasMaxLength(Admin.EmailMaxLength).IsRequired();
        builder.HasIndex(a => a.Email).IsUnique();

        // Ref: Admin.CafeteriaId - Cafeteria.Id [delete: cascade]
        // Índice único em CafeteriaId é o que garante o 1:1 no banco.
        builder.HasOne(a => a.Cafeteria)
            .WithOne(c => c.Admin)
            .HasForeignKey<Admin>(a => a.CafeteriaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
