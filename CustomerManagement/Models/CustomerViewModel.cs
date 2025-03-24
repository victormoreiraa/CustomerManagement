using System;
using System.ComponentModel.DataAnnotations;
using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "O tipo é obrigatório")]
        [Display(Name = "Tipo")]
        public CustomerType Type { get; set; }
        
        [Required(ErrorMessage = "O documento é obrigatório")]
        [Display(Name = "Documento")]
        public string Document { get; set; }
        
        [Display(Name = "Data de Cadastro")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime RegistrationDate { get; set; }
        
        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Display(Name = "Telefone")]
        public string Phone { get; set; }
    }
    
    public class CustomerListViewModel
    {
        public List<CustomerViewModel> Customers { get; set; } = new List<CustomerViewModel>();
        public string SearchTerm { get; set; }
        
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
} 