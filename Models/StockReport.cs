using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPay.Models
{
    public class StockReport
    {
        public ICollection<StockItem> StockItems { get; set; }
        public decimal Total
        {
            get
            {
                return StockItems.Sum(i => i.Amount);
            }
        }
    }
    public class StockItem
    {
        public int StockId { get; set; }
        public string StockName { get; set; }
        public decimal Amount { get; set; }

    }
    public class StockCategoryItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }

    }
    public class StockValueItem
    {
        public string Date { get; set; }
        public decimal Value { get; set; }
    }
}
