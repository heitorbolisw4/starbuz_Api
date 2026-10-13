using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Data.Configurations;

public class CafeteriaConfiguration : IEntityTypeConfiguration<Cafeteria>
{
    public void Configure(EntityTypeBuilder<Cafeteria> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.City).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Address).HasMaxLength(255).IsRequired();
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("now()");

        // Mesmo endereço pode existir em cidades diferentes.
        builder.HasIndex(c => new { c.City, c.Address }).IsUnique();

        // Ref: Cafeteria.ManagerId > Manager.Id [delete: restrict]
        // Restrict: apagar manager com cafeterias falha. Reatribua antes.
        builder.HasOne(c => c.Manager)
            .WithMany(m => m.Cafeterias)
            .HasForeignKey(c => c.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
