using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyPay.Models
{
    public class ProductSet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public string BarCode { get; set; }

        public bool ByCash { get; set; }
        [InverseProperty("ProductSet")]
        public virtual ICollection<ProductSetItem> ProductSetItems { get; set; }
        //
        [ForeignKey("Category")]
        public int categoryId { get; set; }
        public Category Category { get; set; }
        //
    }
    public class ProductSetItem
    {
        public int Id { get; set; }
        public ProductSet ProductSet { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public decimal Qty { get; set; }
    }
}
