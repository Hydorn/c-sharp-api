using Microsoft.EntityFrameworkCore;

public class EntityFWContext : DbContext
{
    public EntityFWContext(DbContextOptions<EntityFWContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.Role).HasDefaultValue("user");
        });
    }
}
