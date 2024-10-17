using Microsoft.EntityFrameworkCore;
using MoniteringBackend.Models;

namespace MoniteringBackend.Data
{
    public class MonitoringContext : DbContext
    {
        public MonitoringContext(DbContextOptions<MonitoringContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ServiceRecord> ServiceRecords { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
                .HasKey(v => v.VehicleId);

            modelBuilder.Entity<ServiceRecord>()
                .HasKey(sr => sr.ServiceRecordId);

            modelBuilder.Entity<ServiceRecord>()
                .HasOne<Vehicle>()
                .WithMany()
                .HasForeignKey(sr => sr.VehicleId);

            modelBuilder.Entity<Driver>()
                .HasKey(d => d.DriverId);

            modelBuilder.Entity<Invoice>()
                .HasKey(i => i.InvoiceId);
        }
    }
}
