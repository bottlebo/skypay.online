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
    public class UnitController : Controller
    {
        private readonly ApplicationDbContext _context;


        public UnitController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Units")]
        [EnableQuery]
        public IQueryable<Unit> Get()
        {

            return _context.Units;//.AsQueryable();
        }
        [HttpGet]
        [Route("Units({key})")]
        [EnableQuery]
        public SingleResult<Unit> Get([FromODataUri] int key)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }

            var unit = _context.Units.Where(m => m.Id == key);

            //if (unit == null)
            //{
            //    return null;
            //}

            return SingleResult.Create(unit);
        }
    }
    
}
