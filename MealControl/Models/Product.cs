using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Product : BaseEntity
    {
        [Required(ErrorMessage="Nome do produto é obrigatório.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage="O valor do produto é obrigatório.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage="Categoria de um produto é obrigatório.")]
        public virtual Category Category { get; set; }
    }
}
