using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyPay.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public string BarCode { get; set; }
        public bool Weighing { get; set; }
        //
        [ForeignKey("Unit")]
        public int unitId { get; set; }
        public virtual Unit Unit { get; set; }
        //
        [ForeignKey("Category")]
        public int categoryId { get; set; }
        public Category Category { get; set; }
        //
        public virtual ICollection<ProductProps> ProductProps { get; set; }

    }
    public class ProductExt
    {
        public Product Product { get; set; }
        //
        public ICollection<ProductProps> ProductProps { get; set; }
        public ICollection<ProductProps> CategoryProductProps { get; set; }

    }
}
