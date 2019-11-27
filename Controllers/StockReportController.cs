using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyPay.Data;
using SkyPay.Models;
using Microsoft.EntityFrameworkCore;

namespace SkyPay.Controllers
{
    [Produces("application/json")]
    [Route("reports")]
    public class StockReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StockReportController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("ShortStock")]
        public IActionResult ShortStock()
        {
            var entries = _context.ProductsInStock
                .Include(z => z.Stock)
                .Where(z => z.Stock.CompanyId == 1)
                .GroupBy(t => new { id = t.StockId, name = t.Stock.Name })
                .Select(g => new StockItem { StockId = g.Key.id, StockName = g.Key.name, Amount = g.Sum(s => s.Qty * s.OutputPrice) })
                .ToList();
            var report = new StockReport
            {
                StockItems = entries
            };
            return Ok(report);
        }
        [HttpGet]
        [Route("ShortSale")]
        public IActionResult ShortSale()
        {
            var entries = _context.DocumentItems
                .Where(z => z.Document.Stock.CompanyId == 1 && z.Document.Type == DocumentType.Out)
                .GroupBy(t => new { id = t.Document.StockId, name = t.Document.Stock.Name })
                .Select(g => new StockItem { StockId = g.Key.id, StockName = g.Key.name, Amount = g.Sum(s => (s.OutputPrice ?? 0) * s.Qty) })
                .ToList();
            var report = new StockReport
            {
                StockItems = entries
            };
            return Ok(report);
        }
        [HttpGet]
        [Route("SaleByCategories")]
        public IActionResult SaleByCategories()
        {
            var entr = _context.DocumentItems
                .Where(z => z.Document.Stock.CompanyId == 1 && z.Document.Type == DocumentType.Out)
                .GroupBy(t => new { id = t.Product.categoryId, name = t.Product.Category.Name })
                .Select(g => new StockCategoryItem { CategoryId = g.Key.id, CategoryName = g.Key.name, Amount = g.Sum(s => s.Qty * (s.OutputPrice ?? 0)) })
                .OrderByDescending(g => g.Amount).Take(10).ToList();
            return Ok(entr);
        }
        [HttpGet]
        [Route("SaleByProducts")]
        public IActionResult SaleByProducts()
        {
            var entr = _context.DocumentItems
                .Where(z => z.Document.Stock.CompanyId == 1 && z.Document.Type == DocumentType.Out)
                .GroupBy(t => new { id = t.ProductId, name = t.Product.Name })
                .Select(g => new StockCategoryItem { CategoryId = g.Key.id, CategoryName = g.Key.name, Amount = g.Sum(s => s.Qty * (s.OutputPrice ?? 0)) })
                .OrderByDescending(g => g.Amount).Take(10).ToList();
            return Ok(entr);
        }
        [HttpGet]
        [Route("StockByCategories")]
        public IActionResult ByCategories()
        {
            var entr = _context.ProductsInStock
                .Where(z => z.Stock.CompanyId == 1)
                .GroupBy(t => new { id = t.Product.categoryId, name = t.Product.Category.Name })
                .Select(g => new StockCategoryItem { CategoryId = g.Key.id, CategoryName = g.Key.name, Amount = g.Sum(s => s.Qty * s.OutputPrice) })
                .OrderByDescending(g => g.Amount).Take(8)
                .ToList();
            return Ok(entr);
        }
        [HttpGet]
        [Route("StockByValues")]
        public IActionResult ByValues()
        {
            try
            {
                var dats = _context.Documents
                    .Where(d => d.Locked == true)
                    .GroupBy(z => z.DocumentDate.ToShortDateString())
                    .OrderBy(g => g.Key)
                    .Select(g => g.Key)
                    .ToList().TakeLast(20);


                var entr = _context.Documents.Include(d => d.Items)
                    .Where(d => d.Stock.CompanyId == 1 && d.Locked == true && d.Type == DocumentType.In)
                    .GroupBy(t => new { date = t.DocumentDate.Date })
                    .OrderBy(g => g.Key.date)
                    .Select(g => new StockValueItem { Date = g.Key.date.ToShortDateString(), Value = g.Sum(s => s.Items.Sum(i => (1 + (int)i.Document.nds / 100m) * ((i.InputPrice * i.Qty) * (1 + i.Document.makrkup / 100)))) })
                    .ToList().TakeLast(20);
                var outrtr = _context.Documents.Include(d => d.Items)
                .Where(d => d.Stock.CompanyId == 1 && d.Locked == true && d.Type == DocumentType.Out)
                .GroupBy(t => new { date = t.DocumentDate.Date })
                .OrderBy(g => g.Key.date)
                .Select(g => new StockValueItem { Date = g.Key.date.ToShortDateString(), Value = g.Sum(s => s.Items.Sum(i => ((i.OutputPrice ?? 0) * i.Qty))) })
                .ToList().TakeLast(20);
                decimal _sum = 0;
                foreach (var item in entr)
                {
                    item.Value += _sum;
                    _sum = item.Value;
                }
                List<StockValueItem> res = new List<StockValueItem>();
                foreach (var d in dats)
                {
                    var r = new StockValueItem();
                    r.Date = d;
                    var ins = entr.Where(w => DateTime.Parse(w.Date) <= DateTime.Parse(d)).Select(w => w.Value).LastOrDefault();
                    var outs = outrtr.Where(o => DateTime.Parse(o.Date) <= DateTime.Parse(d)).Select(o => o.Value).LastOrDefault();

                    r.Value = ins - outs;
                    res.Add(r);
                }
                return Ok(res);
            }
            catch (Exception e)
            {
                var em = e.Message;
                return BadRequest(e);
            }
        }
        [HttpGet]
        [Route("SaleByDays")]
        public IActionResult SaleByDays()
        {
            var entr = _context.Documents.Include(d => d.Items)
                .Where(d => d.Stock.CompanyId == 1 && d.Locked == true && d.Type == DocumentType.Out)
                .GroupBy(t => new { date = t.DocumentDate.Date })
                .OrderBy(g => g.Key.date)
                .Select(g => new StockValueItem { Date = g.Key.date.ToShortDateString(), Value = g.Sum(s => s.Items.Sum(i => ((i.OutputPrice ?? 0) * i.Qty))) })
                .ToList().TakeLast(20);
            return Ok(entr);
        }
    }
}
