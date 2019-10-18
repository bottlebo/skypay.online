using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyPay.Models
{
    public class CategoryPropsValues
    {
        public int Id { get; set; }
        public string Value { get; set; }

        [ForeignKey("CategoryProp")]
        public int categoryPropsId { get; set; }
        public CategoryProps CategoryProp { get; set; }

        [ForeignKey("Product")]
        public int productId { get; set; }
        public Product Product { get; set; }

    }
}
