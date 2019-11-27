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
    public class ProductPropsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductPropsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ProductProps")]
        [EnableQuery]
        public IQueryable<ProductProps> Get()
        {

            return _context.ProductProps;
        }
        [HttpGet]
        [Route("ProductProps({key})")]
        [EnableQuery]
        public SingleResult<ProductProps> Get([FromODataUri] int key)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }

            var props = _context.ProductProps.Where(m => m.Id == key);
            return SingleResult.Create(props);
        }
    }
}
