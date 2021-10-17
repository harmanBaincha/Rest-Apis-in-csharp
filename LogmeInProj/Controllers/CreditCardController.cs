using LogmeInProj.Models;
using Microsoft.AspNetCore.Mvc;
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
    public class CreditCardController : ControllerBase
    {
        private readonly ILogger<CreditCardController> _logger;
        private readonly LastpassContext _context;


        public CreditCardController(ILogger<CreditCardController> logger, LastpassContext context)
        {
            _logger = logger;
            _context = context;

        }

        // GET: api/<CreditCardController>
        [HttpGet]
        public IEnumerable<CreditCard> Get()
        {
            var cards = _context.Creditcard.ToList();

            return cards;
        }

        // GET api/<CreditCardController>/5674-897-2345
        [HttpGet("{number}")]
        public IEnumerable<CreditCard> Get(string number)
        {
            var cards = _context.Creditcard.Where(c => c.CardNumber.Equals(number)).ToList();

            return cards;
        }

        // POST api/<CreditCardController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        
        
        }

        // PUT api/<CreditCardController>/5
        [HttpPut("{number}")]
        public ActionResult Put(string number, [FromBody] CreditCard card)
        {
            var existingCard = _context.Creditcard.Where(c => c.CardNumber.Equals(number)).First();
            if (existingCard != null)
            {
                existingCard.CardType = card.CardType;
                existingCard.CVV = card.CVV;

                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        // DELETE api/<CreditCardController>/5
        [HttpDelete("{number}")]
        public ActionResult Delete(string number)
        {
            var existingCard = _context.Creditcard.Where(c => c.CardNumber == number).FirstOrDefault<CreditCard>();
            if (existingCard != null)
            {
                _context.Remove(existingCard);
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
