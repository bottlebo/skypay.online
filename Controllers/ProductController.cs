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
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Products")]
        [EnableQuery(MaxExpansionDepth = 3, AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Product> Get()
        {
            return _context.Products.Include(z => z.Unit);
        }
        [HttpGet]
        [Route("Products({key})")]
        [EnableQuery(MaxExpansionDepth = 3, AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public SingleResult<Product> Get([FromODataUri] int key)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }
            var product = _context.Products.Include(z => z.Unit).Where(m => m.Id == key);
            return SingleResult.Create(product);
        }
        [HttpGet]
        [Route("Products({key})/ProductProps")]
        [EnableQuery]
        public IQueryable<ProductProps> GetProductProps([FromODataUri] int key)
        {
            return _context.ProductProps.Where(c => c.ProductId == key);
        }
        [HttpGet]
        [Route("Products({key})/ProductCategoryProps")]
        [EnableQuery]
        public ProductPropsExt GetProductCategoryProps([FromODataUri] int key)
        {
            var p = _context.Products.Find(key);

            var q = (from props in _context.CategoryProps
                     from vals in _context.CategoryPropsValues.Where(x => x.categoryPropsId == props.Id && x.productId == p.Id).DefaultIfEmpty()
                     where props.categoryId == p.categoryId
                     select new ProductProps
                     {
                         Id = props.Id,
                         Name = props.Name,
                         Type = props.Type,
                         Value = string.IsNullOrEmpty(vals.Value) ? "" : vals.Value,

                         ProductId = p.Id
                     });
            var r = from props in _context.ProductProps
                    where props.ProductId == key
                    select new ProductProps
                    {
                        Id = props.Id,
                        Name = props.Name,
                        Type = props.Type,
                        Value = string.IsNullOrEmpty(props.Value) ? "" : props.Value,

                        ProductId = p.Id
                    }
                     ;
            ;
            var ext = new ProductPropsExt
            {
                productId = key,
                CategoryProductProps = q.ToList(),
                ProductProps = r.ToList()

            };
            return ext;
        }
        [HttpPost]
        [Route("Product")]
        public async Task<IActionResult> Post([FromBody]ProductExt productext)
        {
            var entity = await _context.Products.AddAsync(productext.Product);
            await _context.SaveChangesAsync();
            foreach (var props in productext.CategoryProductProps)
            {
                var catpropvalue = new CategoryPropsValues
                {
                    categoryPropsId = props.Id,
                    productId = productext.Product.Id,
                    Value = props.Value
                };
                await _context.CategoryPropsValues.AddAsync(catpropvalue);
            }
            foreach (var props in productext.ProductProps)
            {
                var prodpropvalue = new ProductProps
                {
                    Name = props.Name,
                    ProductId = productext.Product.Id,
                    Type = props.Type,
                    Value = props.Value
                };
                await _context.ProductProps.AddAsync(prodpropvalue);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var er = e.Message;
            }
            return Created(Url.RouteUrl(productext.Product.Id), productext.Product.Id);
        }
        [HttpPut]
        [Route("Products({key})")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody]ProductExt productext)
        {
            var existingProduct = _context.Products.Include(x => x.ProductProps).SingleOrDefault(x => x.Id == key);
            if (existingProduct == null)
            {
                return NotFound();
            }
            existingProduct.Name = productext.Product.Name;
            existingProduct.BarCode = productext.Product.BarCode;
            existingProduct.unitId = productext.Product.unitId;
            existingProduct.VendorCode = productext.Product.VendorCode;
            existingProduct.Weighing = productext.Product.Weighing;
            _context.Entry(existingProduct).State = EntityState.Modified;
            var cprops = await _context.CategoryPropsValues.Where(x => x.productId == productext.Product.Id).ToListAsync();

            foreach (var prop in productext.CategoryProductProps)
            {
                var p = cprops.Where(x => x.categoryPropsId == prop.Id).SingleOrDefault();
                if (p == null)
                {
                    if (!string.IsNullOrEmpty(prop.Value))
                    {
                        var catpropvalue = new CategoryPropsValues
                        {
                            categoryPropsId = prop.Id,
                            productId = productext.Product.Id,
                            Value = prop.Value
                        };
                        await _context.CategoryPropsValues.AddAsync(catpropvalue);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(prop.Value))
                    {
                        _context.Entry(p).State = EntityState.Deleted;
                    }
                    else if (prop.Value != p.Value)
                    {
                        p.Value = prop.Value;
                        _context.Entry(p).State = EntityState.Modified;
                    }
                }
            }
            var pprops = await _context.ProductProps.Where(x => x.ProductId == productext.Product.Id).ToListAsync();
            foreach (var prop in pprops)
            {
                var pprop = productext.ProductProps.Where(z => z.Id == prop.Id).SingleOrDefault();
                if (pprop == null)
                {
                    _context.Entry(prop).State = EntityState.Deleted;
                }
                else if (prop.Value != pprop.Value)
                {
                    prop.Value = pprop.Value;
                    _context.Entry(prop).State = EntityState.Modified;
                }
            }
            var addprops = productext.ProductProps.Where(z => z.Id <= 0);
            foreach (var prop in addprops)
            {
                existingProduct.ProductProps.Add(prop);
            }
            //
            await _context.SaveChangesAsync();
            return Ok(key);
        }
        [HttpDelete]
        [Route("Product({key})")]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var existingProduct = await _context.Products.FindAsync(key);
            if (existingProduct == null)
            {
                return NotFound();
            }
            try
            {
                var cprops = _context.CategoryPropsValues.Where(z => z.productId == key);
                _context.CategoryPropsValues.RemoveRange(cprops);
                _context.Entry(existingProduct).State = EntityState.Deleted;
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
