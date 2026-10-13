using BackEnd.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Cafeteria> Cafeterias => Set<Cafeteria>();
    public DbSet<Manager> Managers => Set<Manager>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Carrega todas as classes IEntityTypeConfiguration deste assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
