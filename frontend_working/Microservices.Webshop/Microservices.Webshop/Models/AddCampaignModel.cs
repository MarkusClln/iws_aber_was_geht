using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Webshop.Models
{
    public class AddCampaignModel
    {
        public int MarketingProductId { get; set; }
        public int MarketingDiscount { get; set; }
    }
}
