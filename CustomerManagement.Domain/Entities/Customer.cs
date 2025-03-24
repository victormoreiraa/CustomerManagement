using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Domain.Entities
{
    public class Customer : BaseEntity
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "O tipo é obrigatório")]
        public CustomerType Type { get; set; }
        
        [Required(ErrorMessage = "O documento é obrigatório")]
        public string Document { get; set; }
        
        [Required(ErrorMessage = "A data de cadastro é obrigatória")]
        public DateTime RegistrationDate { get; set; }
        
        [Required(ErrorMessage = "O telefone é obrigatório")]
        [RegularExpression(@"^\(\d{2}\)\s\d{4,5}-\d{4}$", ErrorMessage = "Formato de telefone inválido")]
        public string Phone { get; set; }
    }
    
    public enum CustomerType
    {
        PF = 1,
        PJ = 2
    }
} 