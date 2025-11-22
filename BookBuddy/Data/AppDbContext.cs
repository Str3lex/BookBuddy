using BookBuddy.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BookBuddy.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Knjiga> Knjige => Set<Knjiga>();
    public DbSet<Uporabnik> Uporabniki => Set<Uporabnik>();
    public DbSet<Mnenje> Mnenja => Set<Mnenje>();
    public DbSet<Komentar> Komentarji => Set<Komentar>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfiguracija relacij
        modelBuilder.Entity<Mnenje>()
            .HasOne(m => m.Knjiga)
            .WithMany(k => k.Mnenja)
            .HasForeignKey(m => m.KnjigaId);

        base.OnModelCreating(modelBuilder);
    }
}