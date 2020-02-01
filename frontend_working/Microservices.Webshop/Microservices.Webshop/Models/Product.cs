using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Webshop.Models
{
    public class Product
    {
        public Product() { }
        public Product(string name, string description, string price)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
    }
}
