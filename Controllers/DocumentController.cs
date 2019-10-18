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
using Microsoft.AspNet.OData.Query;
namespace SkyPay.Controllers
{
    [Produces("application/json")]
    [Route("odata")]
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DocumentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Documents")]
        [EnableQuery]
        public IQueryable<Document> Get()
        {
            //System.Threading.Thread.Sleep(3000);

            return _context.Documents;//.AsQueryable();
        }
        [HttpGet]
        [Route("Documents/{type}")]
        [EnableQuery]
        public IQueryable<Document> Get([FromODataUri] string type)
        {
            //System.Threading.Thread.Sleep(3000);

            var t = (int)Enum.Parse(typeof(DocumentType), type);
            return _context.Documents.Where(z => (int)z.Type == t);//.AsQueryable();
        }
        [HttpGet]
        [Route("Documents({key})")]
        [EnableQuery(MaxExpansionDepth = 4, AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public SingleResult<Document> Get([FromODataUri] int key)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }

            var document = _context.Documents.Where(m => m.Id == key);

            //if (company == null)
            //{
            //    return null;
            //}

            return SingleResult.Create(document);
        }
        [HttpPost]
        [Route("Document")]
        public async Task<IActionResult> Post([FromBody]Document document)
        {
            //System.Threading.Thread.Sleep(3000);
            try
            {
                await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync();

                return Ok(document.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        [Route("Documents({key})")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody]Document document)
        {
            //System.Threading.Thread.Sleep(3000);

            var existingDoc = await _context.Documents.FindAsync(key);
            if (existingDoc == null)
            {
                return NotFound();
            }
            try
            {
                existingDoc.AgentId = document.AgentId;
                existingDoc.makrkup = document.makrkup;
                existingDoc.nds = (NdsValue)document.nds;
                existingDoc.Private = document.Private;
                _context.Entry(existingDoc).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(key);
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }
        [HttpPut]
        [Route("Document({key})/Lock")]
        public async Task<IActionResult> Put([FromODataUri] int key)
        {
            //System.Threading.Thread.Sleep(3000);

            var existingDoc = await _context.Documents.Include(z => z.Items).Where(z => z.Id == key).SingleOrDefaultAsync();
            //.FindAsync(key);
            if (existingDoc == null)
            {
                return NotFound();
            }
            //
            if (existingDoc.Type == DocumentType.In)
            {
                try
                {
                    existingDoc.Locked = true;
                    existingDoc.DocumentDate = new DateTime();

                    _context.Entry(existingDoc).State = EntityState.Modified;
                    //
                    foreach (var item in existingDoc.Items)
                    {
                        var _outputPrice = (item.InputPrice * (1 + existingDoc.makrkup / 100.0m)) * (1 + (int)existingDoc.nds / 100.0m);
                        var productInStock = await _context.ProductsInStock.Where(x => x.ProductId == item.ProductId && x.StockId == existingDoc.StockId).SingleOrDefaultAsync();
                        if (productInStock == null)
                        {
                            await _context.ProductsInStock.AddAsync(
                                new ProductInStock
                                {
                                    ProductId = item.ProductId,
                                    StockId = existingDoc.StockId,
                                    OutputPrice = _outputPrice,
                                    Qty = item.Qty,
                                    nds = existingDoc.nds
                                }
                                );
                        }
                        else
                        {
                            productInStock.Qty += item.Qty;
                            productInStock.OutputPrice = _outputPrice;
                            productInStock.nds = existingDoc.nds;

                        }
                    }

                    //var productsInStock = _context.ProductsInStock.Where(z=>z.ProductId == existingDoc.)

                    //
                    await _context.SaveChangesAsync();
                    return Ok(key);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            else
            {
                //var productInSale = new ProductInSale();
                //productInSale.StockId = existingDoc.StockId;
                try
                {
                    existingDoc.Locked = true;
                    existingDoc.DocumentDate = DateTime.Today;
                    _context.Entry(existingDoc).State = EntityState.Modified;
                    foreach (var item in existingDoc.Items)
                    {
                        var productInStock = await _context.ProductsInStock.Where(x => x.ProductId == item.ProductId && x.StockId == existingDoc.StockId).SingleOrDefaultAsync();
                        productInStock.Qty = (productInStock.Qty - item.Qty) >= 0 ? productInStock.Qty - item.Qty : 0;


                    }
                    await _context.SaveChangesAsync();
                    return Ok(key);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
        }



        [HttpDelete]
        [Route("Document({key})")]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            //System.Threading.Thread.Sleep(3000);
            var existingDocument = await _context.Documents.FindAsync(key);
            if (existingDocument == null)
            {
                return NotFound();
            }
            try
            {
                var items = _context.DocumentItems.Where(z => z.DocumentId == existingDocument.Id);
                _context.DocumentItems.RemoveRange(items);
                _context.Entry(existingDocument).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Ok(key);
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }



        [HttpGet]
        [Route("Document({key})/DocumentItems")]
        [EnableQuery(MaxExpansionDepth = 3, AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<DocumentItem> GetDocumentItems([FromODataUri] int key)
        {
            //System.Threading.Thread.Sleep(3000);
            return _context.DocumentItems.Include(z => z.Product).Include(z => z.Product.Unit).Where(z => z.Document.Id == key);
        }
    }
}
