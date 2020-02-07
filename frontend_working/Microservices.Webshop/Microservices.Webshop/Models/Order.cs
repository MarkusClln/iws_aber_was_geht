using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Webshop.Models
{
    public class Order
    {
        public string BillNumber { get; set; }
        public int CustomerId { get; set; }
        public int Price { get; set; }

        public List<Item> Items {get;set;}
    }
}
