using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interfaces.Repositories;
using CustomerManagement.Domain.Interfaces.Services;

namespace CustomerManagement.Application.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        
        public CustomerService(ICustomerRepository customerRepository) : base(customerRepository)
        {
            _customerRepository = customerRepository;
        }
        
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await GetAllAsync();
        }
        
        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
        {
            return await _customerRepository.SearchAsync(searchTerm);
        }
        
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await GetByIdAsync(id);
        }
        
        public async Task CreateCustomerAsync(Customer customer)
        {
            customer.RegistrationDate = DateTime.Now;
            await AddAsync(customer);
        }
        
        public async Task UpdateCustomerAsync(Customer customer)
        {
            await UpdateAsync(customer);
        }
        
        public async Task DeleteCustomerAsync(int id)
        {
            await DeleteAsync(id);
        }
        
        public bool ValidateDocument(string document, CustomerType type)
        {
            if (string.IsNullOrWhiteSpace(document))
                return false;
                
            document = new string(document.Where(char.IsDigit).ToArray());
            
            if (type == CustomerType.PF)
            {
                return ValidateCPF(document);
            }
            else
            {
                return ValidateCNPJ(document);
            }
        }
        
        private bool ValidateCPF(string cpf)
        {
            if (cpf.Length != 11)
                return false;
                
            bool allDigitsEqual = true;
            for (int i = 1; i < cpf.Length; i++)
            {
                if (cpf[i] != cpf[0])
                {
                    allDigitsEqual = false;
                    break;
                }
            }
            
            if (allDigitsEqual)
                return false;
                
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cpf[i].ToString()) * (10 - i);
                
            int remainder = sum % 11;
            int checkDigit1 = remainder < 2 ? 0 : 11 - remainder;
            
            if (int.Parse(cpf[9].ToString()) != checkDigit1)
                return false;
                
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cpf[i].ToString()) * (11 - i);
                
            remainder = sum % 11;
            int checkDigit2 = remainder < 2 ? 0 : 11 - remainder;
            
            return int.Parse(cpf[10].ToString()) == checkDigit2;
        }
        
        private bool ValidateCNPJ(string cnpj)
        {
            if (cnpj.Length != 14)
                return false;
                
            bool allDigitsEqual = true;
            for (int i = 1; i < cnpj.Length; i++)
            {
                if (cnpj[i] != cnpj[0])
                {
                    allDigitsEqual = false;
                    break;
                }
            }
            
            if (allDigitsEqual)
                return false;
                
            int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;
            
            for (int i = 0; i < 12; i++)
                sum += int.Parse(cnpj[i].ToString()) * multiplier1[i];
                
            int remainder = sum % 11;
            int checkDigit1 = remainder < 2 ? 0 : 11 - remainder;
            
            if (int.Parse(cnpj[12].ToString()) != checkDigit1)
                return false;
                
            int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            sum = 0;
            
            for (int i = 0; i < 13; i++)
                sum += int.Parse(cnpj[i].ToString()) * multiplier2[i];
                
            remainder = sum % 11;
            int checkDigit2 = remainder < 2 ? 0 : 11 - remainder;
            
            return int.Parse(cnpj[13].ToString()) == checkDigit2;
        }
        
        public async Task<Customer> GetByDocumentAsync(string document)
        {
            if (string.IsNullOrWhiteSpace(document))
                return null;
            
            string cleanDocument = new string(document.Where(c => char.IsDigit(c)).ToArray());
            
            var customers = await _customerRepository.GetAllAsync();
            return customers.FirstOrDefault(c => 
                new string(c.Document.Where(char.IsDigit).ToArray()) == cleanDocument);
        }
    }
} 