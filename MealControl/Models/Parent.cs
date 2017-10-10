using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Parent : NamedBaseEntity    
    {                      

        [Required(ErrorMessage = "O usuário de acesso é obrigatório.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "O usuário de acesso deve contém de 5 à 50 caracteres.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A senha é obrigatório.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "É obrigatório ter ao menos 1 telefone vinculado ao usuário.")]
        public virtual List<Phone> Phone { get; set; }

        [Required(ErrorMessage = "O status é obrigatório")]
        public virtual Status Status { get; set; }        
        
        [Required(ErrorMessage = "É obrigatório ter ao menos 1 estudante vinculado ao usuário.")]
        public virtual List<Student> Students { get; set; }

    }
}
