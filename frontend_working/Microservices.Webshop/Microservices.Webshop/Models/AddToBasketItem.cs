using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Webshop.Models
{
    public class AddToBasketItem
    {
        public int count { get; set; }
        public int productId { get; set; }
    }
}
