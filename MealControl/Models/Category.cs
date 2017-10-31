using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Category : BaseEntity
    {
        [Required(ErrorMessage="Nome da categoria é obrigatório.")]
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}
