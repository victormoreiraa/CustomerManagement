using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace CustomerManagement.Infrastructure.Persistence.DbContexts
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-KCE56SK\\SQLEXPRESS;Initial Catalog=customerDB;Integrated Security=False;User ID=sa;Password=victor1234;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
} 