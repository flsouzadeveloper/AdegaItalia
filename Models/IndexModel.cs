using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdegaItalia.Models
{
    public class IndexModel
    {
        public string nomeVinho { get; set; }
        public string descricaoItem { get; set; }
        public List<ProdutosModel> listaProdutos { get; set; }
    }
}