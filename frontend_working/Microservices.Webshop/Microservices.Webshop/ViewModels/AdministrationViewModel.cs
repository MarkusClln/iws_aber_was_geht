using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Webshop.ViewModels
{
    public class AdministrationViewModel
    {
        public AdministrationViewModel() { }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }

        public int MarketingProductId { get; set; }
        public int MarketingDiscount { get; set; }
    }
}
