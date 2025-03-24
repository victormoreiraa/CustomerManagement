using CustomerManagement.Application.Services;
using CustomerManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .RegisterServices();
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ICustomerService, CustomerService>();
        }
    }
}
