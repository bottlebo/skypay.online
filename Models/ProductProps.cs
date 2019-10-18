using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyPay.Models
{
    public class ProductProps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public PropsType Type { get; set; }
        //
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
    public class ProductPropsExt
    {
        public int productId { get; set; }
        public ICollection<ProductProps> CategoryProductProps { get; set; }
        public ICollection<ProductProps> ProductProps { get; set; }

    }
}
