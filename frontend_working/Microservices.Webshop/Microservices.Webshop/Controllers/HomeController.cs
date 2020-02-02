using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microservices.Webshop.Models;
using Microservices.Webshop.Services;
using Microservices.Webshop.ViewModels;

namespace Microservices.Webshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RestClient client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            client = new RestClient();
        }

        public async Task<IActionResult> Index()
        {
            var list = new List<Product>();

            //list = await client.GetAllProducts();

            return View(list);
        }

        public IActionResult ProductDetail(string name, string description, string price)
        {
            return View(new Product(name, description, price, "",""));
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

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Administration()
        {
            return View(new AdministrationViewModel());
        }

        [HttpPost]
        public IActionResult AddProduct(AdministrationViewModel viewModel)
        {
            Console.WriteLine(viewModel.ProductName);

            return null;
        }

        [HttpPost]
        public IActionResult AddCampaign(AdministrationViewModel viewModel)
        {
            Console.WriteLine(viewModel.ProductName);

            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
