using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interfaces.Repositories;
using CustomerManagement.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();
                
            return await _dbSet
                .Where(c => c.IsActive && (c.Name.Contains(searchTerm) || c.Document.Contains(searchTerm)))
                .ToListAsync();
        }
    }
} 