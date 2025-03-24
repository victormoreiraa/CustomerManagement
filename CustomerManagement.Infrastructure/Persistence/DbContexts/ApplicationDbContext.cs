using CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using CustomerManagement.Infrastructure.Persistence.Configurations;

namespace CustomerManagement.Infrastructure.Persistence.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasQueryFilter(c => c.IsActive);

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            
        }
    }
}