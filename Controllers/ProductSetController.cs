using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyPay.Data;
using SkyPay.Models;
using Microsoft.AspNet.OData;

namespace SkyPay.Controllers
{
    [Produces("application/json")]
    [Route("odata")]
    public class ProductSetController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductSetController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("ProductSets")]
        [EnableQuery]
        public IQueryable<ProductSet> Get()
        {
            //System.Threading.Thread.Sleep(3000);
            return _context.ProductsSets;//.AsQueryable();
        }
        [HttpGet]
        [Route("ProductSet({key})")]
        [EnableQuery]
        public SingleResult<ProductSet> Get([FromODataUri] int key)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }
            var productset = _context.ProductsSets.Where(m => m.Id == key);

            //if (product == null)
            //{
            //    return null;
            //}

            return SingleResult.Create(productset);
        }
        [HttpPost]
        [Route("ProductSet")]
        public async Task<IActionResult> Post([FromBody]ProductSet productset)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            try
            {
                ProductSet addprset = new ProductSet
                {
                    BarCode = productset.BarCode,
                    ByCash = productset.ByCash,
                    categoryId = productset.categoryId,
                    Name = productset.Name
                };
                var entry = await _context.ProductsSets.AddAsync(addprset);
                await _context.SaveChangesAsync();
                foreach (var item in productset.ProductSetItems)
                {
                    item.ProductSet = addprset;
                    item.Product = await _context.Products.FindAsync(item.ProductId);
                    await _context.ProductsSetItems.AddAsync(item);
                }
                await _context.SaveChangesAsync();
                return Ok(productset.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        [Route("ProductSet({key})")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody]ProductSet productset)
        {
            //System.Threading.Thread.Sleep(3000);
            var existingProductSet = await _context.ProductsSets.FindAsync(key);
            if (existingProductSet == null)
            {
                return NotFound();
            }
            try
            {
                existingProductSet.BarCode = productset.BarCode;
                existingProductSet.ByCash = productset.ByCash;
                existingProductSet.Name = productset.Name;
                existingProductSet.VendorCode = productset.VendorCode;
                _context.Entry(existingProductSet).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(key);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete]
        [Route("ProductSet({key})")]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            //System.Threading.Thread.Sleep(3000);
            var existingProductSet = await _context.ProductsSets.FindAsync(key);
            if (existingProductSet == null)
            {
                return NotFound();
            }
            try
            {
                var items = await _context.ProductsSetItems.Where(z => z.ProductSet.Id == existingProductSet.Id).ToListAsync();
                _context.ProductsSetItems.RemoveRange(items);
                _context.Entry(existingProductSet).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Ok(key);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet]
        [Route("ProductSet({key})/ProductSetItems")]
        [EnableQuery]
        public IQueryable<ProductSetItem> GetProductProps([FromODataUri] int key)
        {
            //System.Threading.Thread.Sleep(3000);
            return _context.ProductsSetItems.Where(c => c.ProductSet.Id == key);

        }
        [HttpPost]
        [Route("ProductSet({key})/AddProduct")]
        public async Task<IActionResult> Post([FromODataUri] int key, [FromBody]ProductSetItem item)
        {
            //System.Threading.Thread.Sleep(3000);

            var productSet = await _context.ProductsSets.FindAsync(key);
            item.Product = await _context.Products.FindAsync(item.ProductId);

            if (productSet == null || item.Product == null)
            {
                return NotFound();
            }
            try
            {
                item.ProductSet = productSet;
                _context.ProductsSetItems.Add(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var ex = e.Message;
            }
            return Ok(key);
        }
        [HttpDelete]
        [Route("ProductSetItem({key})")]
        public async Task<IActionResult> DeleteItem([FromODataUri] int key)
        {
            //System.Threading.Thread.Sleep(3000);

            var item = await _context.ProductsSetItems.FindAsync(key);
            if (item == null)
            {
                return NotFound();
            }
            try
            {
                _context.Entry(item).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Ok(key);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
