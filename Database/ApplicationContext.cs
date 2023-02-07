using Entity;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }

    public DbSet<Work> Works { get; set; }
    public DbSet<Workday> Workdays { get; set; }
    public DbSet<WorkloadTimer> WorkloadTimers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}   

