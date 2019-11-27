using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//
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
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Companies")]
        [EnableQuery]
        public IQueryable<Company> Get()
        {

            return _context.Companies;
        }

        [HttpGet]
        [Route("Companies({key})")]
        [EnableQuery]
        public SingleResult<Company> Get([FromODataUri] int key)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }

            var company = _context.Companies.Where(m => m.Id == key);
            return SingleResult.Create(company);
        }
        [HttpGet]
        [Route("Companies({key})/Categories")]
        [EnableQuery]
        public IQueryable<Category> GetCategories([FromODataUri] int key)
        {
            return _context.Categories.Where(c => c.Company.Id == key);
        }
    }
}
