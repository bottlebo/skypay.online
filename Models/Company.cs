using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SkyPay.Models
{
    public class Company : CompanyBase
    {
        //[InverseProperty(nameof(Category.Company))]
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Agent> Agents { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }

    }
    public abstract class CompanyBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [StringLength(12)]
        [Display(Name = "ИНН")]
        public string INN { get; set; }

        [StringLength(9)]
        [Display(Name = "КПП")]
        public string KPP { get; set; }

        [StringLength(13)]
        [Display(Name = "ОГРН")]
        public string OGRN { get; set; }
        [Display(Name = "Дата выдачи")]
        [DataType(DataType.Date)]

        public DateTime? OgrnDate { get; set; }



        [Display(Name = "Форма собственности")]
        public sobType sob { get; set; }

        //[StringLength(50)]
        [Display(Name = "Юридический дрес")]
        public string Address1 { get; set; }


        [StringLength(100)]
        [Display(Name = "Контактное лицо")]
        public string Contact { get; set; }
        //[StringLength(50)]
        [Display(Name = "Фактический адрес")]
        public string Address { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "e-mail")]
        public string Email { get; set; }
        [StringLength(50)]
        [DataType(DataType.Url)]
        [Display(Name = "Адрес сайта")]
        public string web { get; set; }

        //public string Photo { get; set; }

        //[StringLength(15)]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

    }
    public class Agent : CompanyBase
    {
        public CompanyType Type { get; set; }
        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public NdsValue NDS { get; set; }

    }
}
