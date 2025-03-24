using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Domain.Interfaces.Services
{
    public interface ICustomerService : IBaseService<Customer>
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm);
        Task CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
        bool ValidateDocument(string document, CustomerType type);
        Task<Customer> GetByDocumentAsync(string document);
    }
} 