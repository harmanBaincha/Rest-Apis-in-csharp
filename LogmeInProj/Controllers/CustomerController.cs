using LogmeInProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LogmeInProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly LastpassContext _context;

        public CustomerController(ILogger<CustomerController> logger, LastpassContext context)
        {
            _logger = logger;
            _context = context;

        }

        // GET: api/<CustomerController>
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            var customers = _context.Customer.Include(c => c.Cards).ToList();
            
            return customers;
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public IEnumerable<Customer> Get(int id)
        {
            var customers = _context.Customer.Where(c => c.CustId == id).Include(c => c.Cards).ToList();

            return customers;
        }

        // POST api/<CustomerController>
        [HttpPost]
        public ActionResult Post([FromBody] Customer cust)
        {
            var customer = _context.Customer;
            customer.Add(new Customer { CustId = cust.CustId, Name = cust.Name, Address = cust.Address, DoB = cust.DoB });
            _context.SaveChanges();
            return Ok();
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Customer cust)
        {
            var existingCust = _context.Customer.Where(c => c.CustId == id).FirstOrDefault<Customer>();
            if (existingCust != null)
            {
                existingCust.Name = cust.Name;
                existingCust.Address = cust.Address;

                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
        

        return Ok();
    }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            
            var existingCust = _context.Customer.Where(c => c.CustId == id).FirstOrDefault<Customer>();
            
            
            if (existingCust != null)
            {

                var cards = _context.Creditcard.Where(cc => EF.Property<int>(cc, "CustomerCustId") == id);
                if (cards != null)
                {
                    foreach (var card in cards)
                    {
                        existingCust.Cards.Remove(card);
                        _context.Remove(card);

                    }
                }
                _context.Remove(existingCust);
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
