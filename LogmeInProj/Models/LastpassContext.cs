using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogmeInProj.Models
{
    public class LastpassContext: DbContext
    {
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CreditCard> Creditcard { get; set; }       
        public LastpassContext(DbContextOptions<LastpassContext> options): base(options)
        {

        }
    }
}
