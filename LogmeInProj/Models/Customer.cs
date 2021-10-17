using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogmeInProj.Models
{
    public class Customer
    {
        [Key]
        public int CustId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DoB { get; set; }
        public List<CreditCard> Cards { get; set; }
    }
}
