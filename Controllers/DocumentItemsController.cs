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
    public class DocumentItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DocumentItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("DocumentItems")]
        [EnableQuery]
        public IQueryable<DocumentItem> Get()
        {
            //System.Threading.Thread.Sleep(3000);

            return _context.DocumentItems;//.AsQueryable();
        }
        [HttpGet]
        [Route("DocumentItems({key})")]
        [EnableQuery]
        public SingleResult<DocumentItem> Get([FromODataUri] int key)
        {
            //System.Threading.Thread.Sleep(3000);

            //if (!ModelState.IsValid)
            //{
            //    return null;
            //}

            var _item = _context.DocumentItems.Where(m => m.Id == key);

            //if (company == null)
            //{
            //    return null;
            //}

            return SingleResult.Create(_item);
        }
        [HttpPost]
        [Route("DocumentItem")]
        public async Task<IActionResult> Post([FromBody]DocumentItem item)
        {
            //System.Threading.Thread.Sleep(3000);
            try
            {
                await _context.DocumentItems.AddAsync(item);
                await _context.SaveChangesAsync();

                return Ok(item.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        [Route("DocumentItem({key})")]
        public async Task<IActionResult> Update([FromODataUri] int key, [FromBody]DocumentItem item)
        {
            //System.Threading.Thread.Sleep(3000);
            var existingItem = await _context.DocumentItems.FindAsync(key);
            if (existingItem == null)
            {
                return NotFound();
            }
            try
            {
                existingItem.ProductId = item.ProductId;
                existingItem.InputPrice = item.InputPrice;
                existingItem.Qty = item.Qty;
                _context.Entry(existingItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(key);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpDelete]
        [Route("DocumentItem({key})")]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            //System.Threading.Thread.Sleep(3000);
            var existingItem = await _context.DocumentItems.FindAsync(key);
            if (existingItem == null)
            {
                return NotFound();
            }
            try
            {
                _context.Entry(existingItem).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Ok(key);
            }
            catch (Exception e)
            {
                var er = e.Message;
                return BadRequest(e);
            }
        }
    }
}
