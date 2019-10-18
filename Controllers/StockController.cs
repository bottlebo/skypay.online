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
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Stocks")]
        [EnableQuery]
        public IQueryable<Stock> Get()
        {
            //System.Threading.Thread.Sleep(3000);

            return _context.Stocks;//.AsQueryable();
        }
    }
}
