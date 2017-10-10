using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Student : NamedBaseEntity
    {
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Periodo é obrigatório")]
        public virtual Period Period { get; set; }        
    }
}
