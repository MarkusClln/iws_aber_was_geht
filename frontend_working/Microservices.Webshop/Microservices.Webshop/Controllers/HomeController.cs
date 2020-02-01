using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microservices.Webshop.Models;

namespace Microservices.Webshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var list = new List<Product>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new Product
                {
                    ProductId = i,
                    Name = "Produkt_" + i,
                    Description = "Beschreibung_" + i,
                    Price = i + "€"
                });
            }

            return View(list);
        }

        public IActionResult ProductDetail(string name, string description, string price)
        {
            return View(new Product(name, description, price));
        }

        public IActionResult Basket(bool clear = false)
        {
            var list = new List<Product>();

            if (clear)
            {
                for (int i = 0; i < 3; i++)
                {
                    list.Add(new Product
                    {
                        ProductId = i,
                        Name = "Produkt_" + i,
                        Description = "Beschreibung_" + i,
                        Price = i + "€"
                    });
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    list.Add(new Product
                    {
                        ProductId = i,
                        Name = "Produkt_" + i,
                        Description = "Beschreibung_" + i,
                        Price = i + "€"
                    });
                }
            }

            return View(list);
        }

        public IActionResult AddToBasket()
        {
            return RedirectToAction("Basket");
        }

        public IActionResult RemoveFromBasket(int productId)
        {
            return RedirectToAction("Basket", new { clear = true});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
