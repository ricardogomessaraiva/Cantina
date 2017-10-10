using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class NamedBaseEntity : BaseEntity
    {
        [Required(ErrorMessage ="O nome é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve conter de 5 à 50 caracteres")]        
        public string Name { get; set; }
    }
}
