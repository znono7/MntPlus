using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
        }
        public DbSet<Location>? Locations { get; set; }
        public DbSet<Asset>? Assets { get; set; }
        public DbSet<WorkOrder>? WorkOrders { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Team>? Teams { get; set; }
        public DbSet<WorkOrderHistory>? WorkOrderHistories { get; set; }
        public DbSet<MaintenanceActivities>? MaintenanceActivities { get; set; }
        public DbSet<Schedule>? Schedules { get; set; }



    }
    
}
