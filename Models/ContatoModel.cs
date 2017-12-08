using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdegaItalia.Models
{
    public class ContatoModel
    {
        [Required(ErrorMessage = "Favor preencher o nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Favor preencher o campo email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Favor preencher um email válido")]  
        public string Email { get; set; }

        [Required(ErrorMessage = "Favor preencher o Telefone")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Favor preencher uma mensagem")]
        public string Mensagem { get; set; }
    }
}