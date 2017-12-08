using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdegaItalia.Models
{
    public class ProdutosModel
    {
        public int id { get; set; }
        public string caminhoImage { get; set; }
        public string descricao { get; set; }
        public string detalhe { get; set; }
        public int quantidade { get; set; }
        public bool disponivel { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public string valor { get; set; }
    }
}