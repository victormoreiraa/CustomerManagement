using System.Threading.Tasks;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interfaces.Services;
using CustomerManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;

namespace CustomerManagement.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        
        public async Task<IActionResult> Index(string searchTerm, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            
            IEnumerable<Customer> customers;
            int totalItems;
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var allCustomers = await _customerService.GetAllCustomersAsync();
                totalItems = allCustomers.Count();
                customers = allCustomers
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);
            }
            else
            {
                var searchResults = await _customerService.SearchCustomersAsync(searchTerm);
                totalItems = searchResults.Count();
                customers = searchResults
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);
            }
            
            var customerViewModels = customers.Select(customer => new CustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Type = customer.Type,
                Document = FormatDocument(customer.Document, customer.Type),
                Phone = FormatPhone(customer.Phone),
                RegistrationDate = customer.RegistrationDate
            }).ToList();
            
            var model = new CustomerListViewModel
            {
                Customers = customerViewModels,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
            
            return View(model);
        }
        
        public IActionResult Create() => View();
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            // Remove formatação do documento
            model.Document = CleanDocument(model.Document);
            
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Name = model.Name,
                    Type = model.Type,
                    Document = model.Document,
                    Phone = CleanPhone(model.Phone),
                    RegistrationDate = DateTime.Now,
                    IsActive = true
                };
                
                await _customerService.CreateCustomerAsync(customer);
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Cliente cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            
            return View(model);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            
            var model = new CustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Type = customer.Type,
                Document = FormatDocument(customer.Document, customer.Type),
                RegistrationDate = customer.RegistrationDate,
                Phone = FormatPhone(customer.Phone)
            };
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                
                model.Document = CleanDocument(model.Document);
                
                if (ModelState.IsValid)
                {
                    customer.Name = model.Name;
                    customer.Type = model.Type;
                    customer.Document = model.Document;
                    customer.Phone = CleanPhone(model.Phone);
                    
                    await _customerService.UpdateCustomerAsync(customer);
                    
                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Cliente atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", $"Ocorreu um erro ao salvar: {ex.Message}");
            }
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Cliente excluído com sucesso!";
            }
            catch (System.Exception ex)
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = $"Erro ao excluir cliente: {ex.Message}";
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CheckDocumentAvailability(string document, CustomerType type, int? id = null)
        {
            return Json(new { isValid = true });
        }

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            
            var model = new CustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Type = customer.Type,
                Document = FormatDocument(customer.Document, customer.Type),
                RegistrationDate = customer.RegistrationDate,
                Phone = FormatPhone(customer.Phone)
            };
            
            return View(model);
        }
        
        private string CleanDocument(string document)
        {
            if (string.IsNullOrEmpty(document))
                return string.Empty;
        
            string result = "";
            foreach (char c in document)
            {
                if (char.IsDigit(c))
                    result += c;
            }
        
            return result;
        }

        private bool IsValidDocumentLength(string document, CustomerType type)
        {
            var cleanDoc = CleanDocument(document);
            return (type == CustomerType.PF && cleanDoc.Length == 11) ||
                   (type == CustomerType.PJ && cleanDoc.Length == 14);
        }
            
        private string FormatDocument(string document, CustomerType type)
        {
            if (string.IsNullOrEmpty(document))
                return string.Empty;
        
            string cleanDoc = CleanDocument(document);
            
            if (type == CustomerType.PF && cleanDoc.Length == 11)
            {
                return string.Format("{0}.{1}.{2}-{3}",
                    cleanDoc.Substring(0, 3),
                    cleanDoc.Substring(3, 3),
                    cleanDoc.Substring(6, 3),
                    cleanDoc.Substring(9, 2));
            }
            else if (type == CustomerType.PJ && cleanDoc.Length == 14)
            {
                return string.Format("{0}.{1}.{2}/{3}-{4}",
                    cleanDoc.Substring(0, 2),
                    cleanDoc.Substring(2, 3),
                    cleanDoc.Substring(5, 3),
                    cleanDoc.Substring(8, 4),
                    cleanDoc.Substring(12, 2));
            }
            
            return cleanDoc;
        }

        // Método para limpar telefone
        private string CleanPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return string.Empty;
        
            string result = "";
            foreach (char c in phone)
            {
                if (char.IsDigit(c))
                    result += c;
            }
        
            return result;
        }

        private string FormatPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return string.Empty;
            
            string cleanPhone = CleanPhone(phone);
            
            if (cleanPhone.Length == 11)
            {
                return string.Format("({0}) {1}-{2}",
                    cleanPhone.Substring(0, 2),
                    cleanPhone.Substring(2, 5),
                    cleanPhone.Substring(7, 4));
            }
            else if (cleanPhone.Length == 10)
            {
                return string.Format("({0}) {1}-{2}",
                    cleanPhone.Substring(0, 2),
                    cleanPhone.Substring(2, 4),
                    cleanPhone.Substring(6, 4));
            }
            
            return cleanPhone;
        }
    }
} 