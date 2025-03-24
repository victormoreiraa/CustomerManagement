using CustomerManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerManagement.Domain.Interfaces.Repositories;

namespace CustomerManagement.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<IEnumerable<Customer>> SearchAsync(string searchTerm);
    }
} 