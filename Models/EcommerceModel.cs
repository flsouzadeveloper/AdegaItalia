using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdegaItalia.Models
{
    public class EcommerceModel
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string detalhes { get; set; }
        public string descricaoItem { get; set; }

        [Display(Name = "Quantidade: ")]
        public int quantidade { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public string valor { get; set; }

        public string token { get; set; }
        public string lblErroCarrinho { get; set; }
        public List<ProdutosModel> listaProdutos { get; set; }
        public List<CarrinhoModel> listaProdutosCarrinho { get; set; }
    }
}