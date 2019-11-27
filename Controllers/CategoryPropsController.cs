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
    public class CategoryPropsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryPropsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("CategoryProps")]
        [EnableQuery]
        public IQueryable<CategoryProps> Get()
        {

            return _context.CategoryProps;
        }
        [HttpGet]
        [Route("CategoryProps({key})")]
        [EnableQuery]
        public SingleResult<CategoryProps> Get([FromODataUri] int key)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }

            var props = _context.CategoryProps.Where(m => m.Id == key);
            return SingleResult.Create(props);
        }
        [HttpPost]
        [Route("CategoryProps")]
        public async Task<IActionResult> Post([FromBody]CategoryProps prop)
        {
            var _prop = new CategoryProps
            {
                Name = prop.Name,
                Type = prop.Type,
                categoryId = prop.categoryId
            };
            var entity = await _context.CategoryProps.AddAsync(prop);
            await _context.SaveChangesAsync();
            return Created(Url.RouteUrl(prop.Id), prop.Id);
        }
    }
}
