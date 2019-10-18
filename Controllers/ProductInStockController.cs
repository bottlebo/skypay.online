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
    public class ProductInStockController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductInStockController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("ProductInStock")]
        [EnableQuery]
        public IQueryable<ProductInStock> Get()
        {
            //System.Threading.Thread.Sleep(3000);

            return _context.ProductsInStock;//.AsQueryable();
        }
    }
}
