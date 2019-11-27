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
    public class AgentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgentController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Agents")]
        [EnableQuery]
        public IQueryable<Agent> Get()
        {

            return _context.Agents;
        }
        [HttpGet]
        [Route("Agents/{type}")]
        [EnableQuery]
        public IQueryable<Agent> Get([FromODataUri] string type)
        {

            var t = (int)Enum.Parse(typeof(CompanyType), type);
            return _context.Agents.Where(z => (int)z.Type == t);
        }
        [HttpPost]
        [Route("Agents")]
        public async Task<IActionResult> Post([FromBody]Agent agent)
        {
            try
            {
                await _context.AddAsync(agent);
                await _context.SaveChangesAsync();

                return Ok(1);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPut]
        [Route("Agents({key})")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody]Agent agent)
        {
            var existingAgent = await _context.Agents.FindAsync(key);
            if (existingAgent == null)
            {
                return NotFound();
            }
            try
            {
                existingAgent.Address = agent.Address;
                existingAgent.Address1 = agent.Address1;
                existingAgent.Contact = agent.Contact;
                existingAgent.Email = agent.Email;
                existingAgent.INN = agent.INN;
                existingAgent.KPP = agent.KPP;
                existingAgent.Name = agent.Name;
                existingAgent.NDS = agent.NDS;
                existingAgent.OGRN = agent.OGRN;
                existingAgent.OgrnDate = agent.OgrnDate;
                existingAgent.Phone = agent.Phone;
                existingAgent.sob = agent.sob;
                existingAgent.web = agent.web;
                _context.Entry(existingAgent).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(key);
            }
            catch (Exception e)
            {
                return BadRequest(e);

            }
        }
    }
}
