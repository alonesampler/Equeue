using EQ.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EQ.WebApi.Infrastructure.EfCore;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<TicketCounter> TicketCounters { get; set; }
    public DbSet<Window> Windows { get; set; }
    public DbSet<WindowSection> WindowsSections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
}
