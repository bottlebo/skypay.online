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
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;

namespace SkyPay.Controllers
{
    [Produces("application/json")]
    [Route("odata")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Categories")]
        [EnableQuery]
        public IQueryable<Category> Get()
        {

            return _context.Categories;//.AsQueryable();
        }

        // GET: api/Products/5
        [HttpGet]
        [Route("Categories({key})")]
        [EnableQuery]
        public SingleResult<Category> Get([FromODataUri] int key)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }

            var category = _context.Categories.Where(m => m.Id == key);

            //if (category == null)
            //{
            //    return null;
            //}

            return SingleResult.Create(category);
        }
        [HttpGet]
        [Route("Categories({key})/Company")]
        [EnableQuery]
        public SingleResult<Company> GetCompany([FromODataUri] int key)
        {
            //var _c = _context.Categories.Include(c=>c.Company).SingleOrDefault(c => c.Id == key).Company;
            var c = _context.Categories.Where(x => x.Id == key).Select(x => x.Company);
            //return _context.Categories.Include(c => c.Company).SingleOrDefault(c => c.Id == key).Company;
            return SingleResult.Create(c);
        }
        [HttpGet]
        [Route("Categories({key})/Properties")]
        [EnableQuery]
        public async Task<List<CategoryProps>> GetProperties([FromODataUri] int key)
        {
            //var _c = _context.Categories.Include(c=>c.Company).SingleOrDefault(c => c.Id == key).Company;
            var props = await _context.CategoryProps.Where(x => x.categoryId == key).ToListAsync();
            //return _context.Categories.Include(c => c.Company).SingleOrDefault(c => c.Id == key).Company;
            ProductProps pr = new ProductProps
            {
                 
            };
            return props;
        }
        [HttpPost]
        [Route("Categories")]
        public async Task<IActionResult> Post([FromBody]Category category)
        {
            var _category = new Category
            {
                //Id = category.Id,
                CategoryProps = category.CategoryProps,
                Company = category.Company,
                CompanyId = category.CompanyId,
                HasChild = category.HasChild,
                Name = category.Name,
                parentId = category.parentId,
                Products = category.Products
            };
            var entity = await _context.Categories.AddAsync(category);
            var _parent = _context.Categories.Where(x => x.Id == category.parentId).SingleOrDefault();
            if (_parent != null)
                _parent.HasChild = true;
            await _context.SaveChangesAsync();
            //_context.Categories.
            //return Ok()
            return Created(Url.RouteUrl(category.Id), category.Id);
        }
        [HttpPut]
        [Route("Categories({key})")]
        public IActionResult Put([FromODataUri] int key, [FromBody]Category category)
        {
            var existingCat = _context.Categories.Include(x=>x.CategoryProps).SingleOrDefault(x=>x.Id == key);
            if (existingCat == null)
            {
                return NotFound();
            }
            existingCat.Name = category.Name;

            _context.Entry(existingCat).State = EntityState.Modified;
            foreach (var props in existingCat.CategoryProps)
            {
                var c = category.CategoryProps.FirstOrDefault(x => x.Id == props.Id);
                if (c == null)
                {
                    //existingCat.CategoryProps.Remove(props);
                    _context.Entry(props).State = EntityState.Deleted;
                }
                else if (!props.Equals(c))
                {
                    props.Name = c.Name;
                    props.Type = c.Type;
                    _context.Entry(props).State = EntityState.Modified;
                }
            }
            var addprops = category.CategoryProps.Where(x => x.Id <= 0);
            foreach (var prop in addprops)
            {
                existingCat.CategoryProps.Add(prop);
            }
            _context.SaveChanges();
            return Ok(key);
        }
        [HttpDelete]
        [Route("Category({key})")]
        public IActionResult Delete([FromODataUri] int key)
        {
            var existingCat = _context.Categories.Find(key);
            _context.Entry(existingCat).State = EntityState.Deleted;
            if (existingCat.HasChild)
                deleteChild(existingCat);
            var _anychilds= _context.Categories.Where(x => x.parentId == existingCat.parentId).Count();
            if (_anychilds <= 1)
            {
                var _parent = _context.Categories.Where(x => x.Id == existingCat.parentId).SingleOrDefault();

                _parent.HasChild = false;
            }
            _context.SaveChanges();
            return Ok(key);
        }
        protected void deleteChild(Category cat)
        {
            var childs = _context.Categories.Where(c => c.parentId == cat.Id);
            foreach(var child in childs)
            {
                _context.Entry(child).State = EntityState.Deleted;
                if (child.HasChild)
                    deleteChild(child);
            }
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
