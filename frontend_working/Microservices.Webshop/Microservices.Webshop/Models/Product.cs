using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Webshop.Models
{
    public class Product
    {
        public Product() { }
        public Product(int productId, string name, string description, int price, string quantity, int discount)
        {
            this.ProductId = productId;
            this.ProductName = name;
            this.ProductDescription = description;
            this.ProductPrice = price;
            this.ProductQuantity = quantity;
            this.ProductDiscount = discount;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public string ProductQuantity { get; set; }
        public int ProductDiscount { get; set; }
    }
}
