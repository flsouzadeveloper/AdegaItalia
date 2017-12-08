using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdegaItalia.Models
{
    [Serializable]
    public class CarrinhoModel
    {
        public int itemId { get; set; }
        public string itemDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public string itemAmount { get; set; }
        public int itemQuantity { get; set; }
        public string token { get; set; }
    }
}