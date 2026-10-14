using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Data.Configurations;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name).HasMaxLength(Manager.NameMaxLength).IsRequired();
        builder.Property(m => m.Phone).HasMaxLength(Manager.PhoneMaxLength).IsRequired();
        builder.Property(m => m.Email).HasMaxLength(Manager.EmailMaxLength).IsRequired();
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("now()");
        builder.HasIndex(m => m.Email).IsUnique();

        // Coleção exposta como IReadOnlyCollection: EF escreve direto no campo _cafeterias.
        builder.Navigation(m => m.Cafeterias).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
