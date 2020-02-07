using Microservices.Webshop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Webshop.Services
{
    public class RestClient
    {
        public HttpClient client;
        public string BaseUrl = "http://localhost:3005/";
        public int CustomerId = 1;

        public RestClient()
        {
            client = new HttpClient();
        }

        //DONE
        public async Task<List<Product>> GetAllProducts()
        {
            var list = new List<Product>();

            var httpResponse = await client.GetAsync(BaseUrl + "api/allProducts");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            list = JsonConvert.DeserializeObject<List<Product>>(content);

            return list;
        }

        //DONE
        public async Task<Basket> GetBasket()
        {
            var httpResponse = await client.GetAsync(BaseUrl +"api/showBasket/"+ CustomerId);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var basket = JsonConvert.DeserializeObject<Basket>(content);

            return basket;
        }

        //DONE
        public async Task CheckoutBasket()
        {
            var httpResponse = await client.PostAsync(BaseUrl + "api/checkout/" + CustomerId, new StringContent("", Encoding.Default, "application/json"));
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot add a todo task");
            }
        }

        //DONE
        public async Task<Product> GetProduct(int productId)
        {
            var httpResponse = await client.GetAsync(BaseUrl + "product/" + productId.ToString());

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(content);

            return product;
        }

        //DONE
        public async Task AddToBasket(AddToBasketItem product)
        {
            var content = JsonConvert.SerializeObject(product);
            var httpResponse = await client.PostAsync(BaseUrl + "basket/" + CustomerId, new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot add a todo task");
            }
        }

        //DONE
        public async Task ClearBasket()
        {
            
            var httpResponse = await client.DeleteAsync(BaseUrl + "basket/clear/" + CustomerId);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot add a todo task");
            }
        }

        public async Task AddProduct(AddProductModel model)
        {
            var content = JsonConvert.SerializeObject(model);
            var httpResponse = await client.PostAsync(BaseUrl + "product/", new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot add a todo task");
            }
        }

        public async Task AddCampaign(AddCampaignModel model)
        {
            var content = JsonConvert.SerializeObject(model);
            var httpResponse = await client.PostAsync(BaseUrl + "marketing/", new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot add a todo task");
            }
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var httpResponse = await client.GetAsync(BaseUrl + "payment/" + CustomerId);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var orderList = JsonConvert.DeserializeObject<List<Order>>(content);

            return orderList;
        }

    }
}
