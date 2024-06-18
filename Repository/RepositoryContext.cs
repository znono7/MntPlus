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
            modelBuilder.Entity<Asset>() 
                .HasOne(a => a.Parent) 
                .WithMany(l => l.Assets)
                .HasForeignKey(a => a.AssetParent)
                .OnDelete(DeleteBehavior.Cascade);
             
            modelBuilder.Entity<Inventory>() 
                .HasOne(i => i.Part)
                .WithMany(p => p.Inventories)
                .HasForeignKey(i => i.PartID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Meter>()
                .HasMany(m => m.MeterReadings)
                .WithOne(mr => mr.Meter)
                .HasForeignKey(mr => mr.MeterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CheckListItem>()
                .HasOne(c => c.IndividualTask)
                .WithMany(i => i.CheckListItems)
                .HasForeignKey(c => c.IndividualTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CheckListItem>()
                .HasOne(c => c.CheckList)
                .WithMany(w => w.CheckListItems)
                .HasForeignKey(c => c.CheckListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkOrder>()
                .HasMany(wo => wo.WorkOrderHistories)
                .WithOne(woh => woh.WorkOrder)
                .HasForeignKey(woh => woh.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            
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
        public DbSet<Inventory>? Inventories { get; set; }
        public DbSet<Part>? Parts { get; set; }
        public DbSet<LinkPart>? LinkParts { get; set; }
        public DbSet<Request>? Requests { get; set; } 

        public DbSet<Meter>? Meters  { get; set; }
        public DbSet<MeterReading>? MeterReadings { get; set; }
        public DbSet<CheckList>? CheckLists { get; set; }
        public DbSet<CheckListItem>? CheckListItems { get; set; }
        public DbSet<IndividualTask>? IndividualTasks { get; set; }







    }

}
