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
        public DbSet<Role>? Roles { get; set; }
        public DbSet<UserTeam>? UserTeams { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }
        public DbSet<WorkOrderHistory>? WorkOrderHistories { get; set; }
        public DbSet<PreventiveMaintenance>? PreventiveMaintenances { get; set; }
        public DbSet<PreventiveMaintenanceHistory>? PreventiveMaintenanceHistories { get; set; }
        public DbSet<Schedule>? Schedules { get; set; }
        public DbSet<WorkTask>? WorkTasks { get; set; }
        public DbSet<Inventory>? Inventories { get; set; }
        public DbSet<Part>? Parts { get; set; }
        public DbSet<LinkPart>? LinkParts { get; set; }
        public DbSet<Request>? Requests { get; set; }






    }

}
