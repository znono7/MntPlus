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
            //modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
        }
        public DbSet<Assignee>? Assignees { get; set; }
        public DbSet<EquipmentClass>? EquipmentClasses { get; set; }
        public DbSet<EquipmentDepartment>? EquipmentDepartments { get; set; }
        public DbSet<EquipmentStatus>? EquipmentStatuses { get; set; }
        public DbSet<EquipmentType>? EquipmentTypes { get; set; }
        public DbSet<Organization>? Organizations { get; set; }
        public DbSet<Equipment>? Equipment { get; set; }


        
    }
    
}
