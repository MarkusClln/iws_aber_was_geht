using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Webshop.Models
{
    public class Product
    {
        public Product() { }
        public Product(int productId, string name, string description, int price, int quantity, int discount, int picNum)
        {
            this.ProductId = productId;
            this.ProductName = name;
            this.ProductDescription = description;
            this.ProductPrice = price;
            this.ProductQuantity = quantity;
            this.ProductDiscount = discount;
            this.PicNum = picNum;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductDiscount { get; set; }
        public int PicNum { get; set; }
    }
}
