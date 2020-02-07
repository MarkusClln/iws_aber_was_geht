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

            list = await client.GetAllProducts();

            return View(list);
        }

        public IActionResult ProductDetail(string name, string description, int price, int productId)
        {
            return View(new Product(productId, name, description, price, "",0));
        }

        public async Task<IActionResult> Basket()
        {
            var basket = await client.GetBasket();

            return View(basket);
        }

        public async Task<IActionResult> AddToBasket(int productId)
        {
            await client.AddToBasket(new AddToBasketItem {count = 1, productId = productId });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromBasket(int productId)
        {
            //await client.//RemoveBasketItem(productId);
            return RedirectToAction("Basket");
        }

        public async Task<IActionResult> ClearBasket()
        {
            await client.ClearBasket();
            return RedirectToAction("Basket");
        }

        public async Task<IActionResult> Checkout()
        {
            await client.CheckoutBasket();
            return RedirectToAction("Basket");
        }

        public IActionResult Administration()
        {
            return View(new AdministrationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AdministrationViewModel viewModel)
        {
            await client.AddProduct(new AddProductModel { ProductId = viewModel.ProductId, 
                ProductName = viewModel.ProductName, 
                ProductDescription = viewModel.ProductDescription, 
                ProductPrice = viewModel.ProductPrice, 
                ProductQuantity = viewModel.ProductQuantity});
            return RedirectToAction("Administration");
        }

        [HttpPost]
        public async Task<IActionResult> AddCampaign(AdministrationViewModel viewModel)
        {
            await client.AddCampaign(new AddCampaignModel
            {
                MarketingDiscount = viewModel.MarketingDiscount,
                MarketingProductId = viewModel.MarketingProductId,
                
            });
            return RedirectToAction("Administration");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
