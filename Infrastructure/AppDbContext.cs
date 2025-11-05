using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }
    public DbSet<Participant> Participants => Set<Participant>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
       
        mb.Entity<Participant>()
          .HasIndex(p => new { p.Nombre, p.Apellidos });

        mb.Entity<Participant>()
          .HasIndex(p => p.Email)
          .IsUnique(); 
    }
}
