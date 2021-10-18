using LogmeInProj.Helpers;
using LogmeInProj.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;




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
        public IActionResult Get()
        {
            try
            {
                var cards = _context.Creditcard.ToList();
                if (cards != null)
                {
                    List<CreditCard> decCardsList = new List<CreditCard>();
                    foreach (var card in cards)
                    {
                        var decCardNumber = AesOperation.DecryptString(Keys.SKeys, card.CardNumber);
                        var decCVV = AesOperation.DecryptString(Keys.SKeys, card.CVV);
                        var newCard = new CreditCard { CreditCardID = card.CreditCardID, CardNumber = decCardNumber, CardType = card.CardType, CVV = decCVV, ExpiryDate=card.ExpiryDate};
                        decCardsList.Add(newCard);
                    }
                    return Ok(decCardsList);
                }
                else
                {
                    return NotFound("Card not found");
                }


            }
            catch (Exception e)
            {
                _logger.LogError("Exception thrown in Creditcard get operation  :" + e);
                return BadRequest("Exception thrown in Creditcard get operation  :" + e);
            }
        }

        // GET api/<CreditCardController>/2
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var cards = _context.Creditcard.Where(c => c.CreditCardID == id).ToList();
                if (cards != null)
                {
                    List<CreditCard> decCardsList = new List<CreditCard>();
                    foreach (var card in cards)
                    {
                        var decCardNumber = AesOperation.DecryptString(Keys.SKeys, card.CardNumber);
                        var decCVV = AesOperation.DecryptString(Keys.SKeys, card.CVV);
                        var newCard = new CreditCard { CreditCardID = card.CreditCardID, CardNumber = decCardNumber, CardType = card.CardType, CVV = decCVV, ExpiryDate=card.ExpiryDate};
                        decCardsList.Add(newCard);
                    }
                    return Ok(decCardsList);
                }
                else
                {
                    return NotFound("Card not found");
                }

            }
            catch (Exception e)
            {
                _logger.LogError("Exception thrown in Creditcard get operation with id " + id + " :" + e);
                return BadRequest("Exception thrown in Creditcard get operation with id " + id + " :" + e);
            }
        }



        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<CreditCard> patchEntity)
        {
            try
            {
                var entity = _context.Creditcard.FirstOrDefault(card => card.CreditCardID == id);

                if (entity == null)
                {
                    return NotFound("Not Found");
                }

                foreach (var op in patchEntity.Operations)
                {
                    if (op.path.ToString() == "/CardNumber" || op.path.ToString() == "/CVV")
                    {
                        op.value = AesOperation.EncryptString(Keys.SKeys, op.value.ToString());
                    }
                }

                patchEntity.ApplyTo(entity, ModelState); 
                _context.SaveChanges();
                return Ok("Card updated");
            }
            catch (Exception e)
            {
                _logger.LogError("Exception thrown in Creditcard patch operation with id " + id + " :" + e);
                return BadRequest("Exception thrown in Creditcard patch operation with id " + id + " :" + e);
            }
        }


        // DELETE api/<CreditCardController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existingCard = _context.Creditcard.Where(c => c.CreditCardID == id).FirstOrDefault<CreditCard>();
                if (existingCard != null)
                {
                    _context.Remove(existingCard);
                    _context.SaveChanges();
                    return Ok("Card Removed");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception thrown in Creditcard delete operation with id " + id + " :" + e);
                return BadRequest("Exception thrown in Creditcard delete operation with id " + id + " :" + e);

            }
        }
    }
}
