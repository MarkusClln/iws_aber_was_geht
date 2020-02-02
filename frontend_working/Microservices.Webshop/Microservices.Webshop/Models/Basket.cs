using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Webshop.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public List<Item> Items { get; set; }
    }
}
