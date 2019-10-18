using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyPay.Models
{
    public class CategoryProps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PropsType Type { get; set; }

        [ForeignKey("Category")]
        public int categoryId { get; set; }
        public Category Category { get; set; }
        public bool Equals(CategoryProps p)
        {
            return Name == p.Name && Type == p.Type;
        }
    }
    //[NotMapped]
}
