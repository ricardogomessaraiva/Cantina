using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Phone : BaseEntity
    {                
        [Required(ErrorMessage ="O número do telefone é obrigatório")]
        public long Number { get; set; }
    }
}
