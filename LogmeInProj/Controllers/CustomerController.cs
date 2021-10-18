using LogmeInProj.Helpers;
using LogmeInProj.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;


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
        public IActionResult Get()
        {
            
            try
            {
                var customers = _context.Customer.Include(c => c.Cards).ToList();
                List<Customer> custList = new List<Customer>();
                foreach(var cust in customers)
                {
                    var cards = cust.Cards;
                    
                    if (cards != null)
                    {
                        List<CreditCard> decCardsList = new List<CreditCard>();
                        foreach (var card in cards)
                        {
                            var decCardNumber = AesOperation.DecryptString(Keys.SKeys, card.CardNumber);
                            var decCVV = AesOperation.DecryptString(Keys.SKeys, card.CVV);
                            var newCard = new CreditCard { CreditCardID = card.CreditCardID, CardNumber =decCardNumber, CardType = card.CardType, CVV = decCVV, ExpiryDate=card.ExpiryDate };
                            decCardsList.Add(newCard);
                        }
                        var newCust = new Customer { CustId = cust.CustId, Name = cust.Name, Address = cust.Address, DoB = cust.DoB, Cards = decCardsList };
                        custList.Add(newCust);
                    }
                    else
                    {
                        var newCust = new Customer { CustId = cust.CustId, Name = cust.Name, Address = cust.Address, DoB = cust.DoB };
                        custList.Add(newCust);
                    }
                }
                return Ok(custList);
            }
            catch(Exception e){
                _logger.LogError("Exception thrown in Customer get operation" + e);
                return BadRequest("Exception thrown in Customer get operation" + e);
            }
            
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var customers = _context.Customer.Where(c => c.CustId == id).Include(c => c.Cards).ToList();
                List<Customer> custList = new List<Customer>();
                foreach (var cust in customers)
                {
                    var cards = cust.Cards;

                    if (cards != null)
                    {
                        List<CreditCard> decCardsList = new List<CreditCard>();
                        foreach (var card in cards)
                        {
                            var decCardNumber = AesOperation.DecryptString(Keys.SKeys, card.CardNumber);
                            var decCVV = AesOperation.DecryptString(Keys.SKeys, card.CVV);
                            var newCard = new CreditCard { CreditCardID = card.CreditCardID, CardNumber = decCardNumber, CardType = card.CardType, CVV = decCVV, ExpiryDate=card.ExpiryDate };
                            decCardsList.Add(newCard);
                        }
                        var newCust = new Customer { CustId = cust.CustId, Name = cust.Name, Address = cust.Address, DoB = cust.DoB, Cards = decCardsList };
                        custList.Add(newCust);
                    }
                    else
                    {
                        var newCust = new Customer { CustId = cust.CustId, Name = cust.Name, Address = cust.Address, DoB = cust.DoB };
                        custList.Add(newCust);
                    }
                }
                return Ok(custList);


                
            }
            catch(Exception e)
            {
                _logger.LogError("Exception thrown in Customer get operation with id "+ id +" :"+ e);
                return BadRequest("Exception thrown in Customer get operation with id " + id + " :" + e);
            }
        }

        // POST api/<CustomerController>
        [HttpPost]
        public ActionResult Post([FromBody] Customer cust)
        {
            try
            {
                var customer = _context.Customer;
                var cards = cust.Cards;
                if (cards != null)
                {
                    List<CreditCard> encCardsList = new List<CreditCard>();
                    foreach (var card in cards)
                    {
                        var encrpytcardNumber = AesOperation.EncryptString(Keys.SKeys, card.CardNumber);
                        var encrpytCVV = AesOperation.EncryptString(Keys.SKeys, card.CVV);
                        var newCard = new CreditCard { CreditCardID = card.CreditCardID, CardNumber = encrpytcardNumber, CardType = card.CardType, CVV = encrpytCVV, ExpiryDate=card.ExpiryDate };
                        encCardsList.Add(newCard);
                    }
                    customer.Add(new Customer { CustId = cust.CustId, Name = cust.Name, Address = cust.Address, DoB = cust.DoB, Cards = encCardsList });
                    _context.SaveChanges();
                }
                else
                {
                    customer.Add(new Customer { CustId = cust.CustId, Name = cust.Name, Address = cust.Address, DoB = cust.DoB });
                    _context.SaveChanges();
                }
                return Ok("Customer added");
            }
            catch(Exception e)
            {
                _logger.LogError("Exception thrown in Customer post operation with object " + cust + " :" + e);
                return BadRequest("Exception thrown in Customer post operation with object " + cust + " :" + e);
            }
        }

        

        //Patch request
        [HttpPatch("{id}")]
        public IActionResult JsonPatchWithModelState(int id, [FromBody] JsonPatchDocument<Customer> patchEntity)
        {
            try
            {
                var customer = _context.Customer.Include(c => c.Cards).FirstOrDefault(cust => cust.CustId == id);

                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                else
                {
                    var i = 0;
                    foreach (var op in patchEntity.Operations)
                    {
                        if (op.path.ToString() == "/Cards/"+i+"/CardNumber" || op.path.ToString() == "/Cards/" + i + "/CVV")
                        {
                            op.value = AesOperation.EncryptString(Keys.SKeys, op.value.ToString());
                        }
                        i = i + 1;
                    }
                }

                patchEntity.ApplyTo(customer, ModelState); // Must have Microsoft.AspNetCore.Mvc.NewtonsoftJson installed
                _context.SaveChanges();
                return Ok("Customer updated");
            }
            catch(Exception e)
            {
                _logger.LogError("Exception thrown in Customer patch operation with id " + id + " :" + e);
                return BadRequest("Exception thrown in Customer patch operation with id " + id + " :" + e);
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
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
                return Ok("Customer deleted");
            }
            catch(Exception e)
            {
                _logger.LogError("Exception thrown in Customer delete operation with id " + id + " :" + e);              
                return BadRequest("Exception thrown in Customer delete operation with id " + id + " :" + e);
            }
            
        }
    }
}
