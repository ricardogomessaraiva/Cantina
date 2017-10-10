using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage="Nome do responsável é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage="Usuário de acesso é obrigatório.")]
        public string UserName { get; set; }

        [Required(ErrorMessage="Senha de acesso é obrigatório.")]
        public string Password { get; set; }

        [Required(ErrorMessage="E-mail do usuário é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage="O tipo do usuário é obrigatório.")]
        public virtual UserType Type { get; set; }

        [Required(ErrorMessage="O status é do usuário é obrigatório.")]
        public virtual Status Status { get; set; }


    }
}
