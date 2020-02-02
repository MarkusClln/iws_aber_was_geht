using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Webshop.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int CustomerBasketId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
