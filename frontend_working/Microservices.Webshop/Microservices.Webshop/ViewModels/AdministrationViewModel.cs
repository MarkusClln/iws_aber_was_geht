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
        public string ProductPrice { get; set; }

        public string MarketingProductId { get; set; }
        public string MarketingDiscount { get; set; }
    }
}
