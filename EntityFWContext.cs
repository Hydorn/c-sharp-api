using Microsoft.EntityFrameworkCore;

public class EntityFWContext : DbContext
{
    public EntityFWContext(DbContextOptions<EntityFWContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
}
