using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyPay.Models
{
    public class ProductInStock
    {
        public int Id { get; set; }
        //
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        //
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //
        public decimal Qty { get; set; }
        public decimal OutputPrice { get; set; }
        //
        public NdsValue nds { get; set; }

    }
}
