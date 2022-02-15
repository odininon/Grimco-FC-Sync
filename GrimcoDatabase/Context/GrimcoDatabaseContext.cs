using GrimcoLib.Models;
using Microsoft.EntityFrameworkCore;

namespace GrimcoDatabase.Context;

public class GrimcoDatabaseContext : DbContext
{
    public DbSet<Duty> Duties { get; set; }
    public DbSet<Quest> Quests { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<UnlockedDuty> UnlockedDuties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=Grimco;Username=postgres;Password=postgres;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quest>().HasIndex(q => q.Name).IsUnique(true);
        modelBuilder.Entity<Duty>().HasIndex(i => i.Name).IsUnique(true);
    }
}