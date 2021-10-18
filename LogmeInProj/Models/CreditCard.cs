using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace LogmeInProj.Models
{
    public class CreditCard
    {
       
        [Key]
        public int CreditCardID { get; set; }

        public string CardNumber { get; set; }
        public Type CardType { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }

    }

    public enum Type
    {
        Amex, Visa, MasterCard
    }
}
