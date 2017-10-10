using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Status : BaseEntity
    {        
        [MaxLength(20)]
        [Required]
        public string Description { get; set; }
    }
}
