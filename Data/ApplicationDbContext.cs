using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IT15_Trojan_B.Models; // Ensure all models are included

namespace IT15_Trojan_B.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define database tables correctly

        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<SafetyEquipment> SafetyEquipments { get; set; }
        public DbSet<ToolEquipment> ToolEquipments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SecurityLog> SecurityLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Material>()
           .Property(m => m.Price)
           .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SafetyEquipment>()
        .Property(se => se.Price)
        .HasColumnType("decimal(18,2)");
        }
    }
}