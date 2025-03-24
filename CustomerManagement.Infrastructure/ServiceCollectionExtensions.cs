using CustomerManagement.Domain.Interfaces.Repositories;
using CustomerManagement.Infrastructure.Persistence.DbContexts;
using CustomerManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .RegisterServices()
                .RegisterRepositories()
                .RegisterDbContext(configuration);
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<ICustomerRepository, CustomerRepository>();
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            return services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(connectionString));
        }
    }
}
