using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace SkyPay.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasChild { get; set; }
        public int? parentId { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<CategoryProps> CategoryProps { get; set; }
       
    }
}
