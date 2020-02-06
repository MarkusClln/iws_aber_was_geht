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

        public RestClient()
        {
            client = new HttpClient();
        }

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

        public async Task<Basket> GetBasket(int customerId)
        {
            var httpResponse = await client.GetAsync(BaseUrl +"api/showBasket/"+ customerId.ToString());

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var basket = JsonConvert.DeserializeObject<Basket>(content);

            return basket;
        }

        public void CheckoutBasket()
        {

        }

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

        public async Task AddToBasket(Item product, int customerId)
        {
            var content = JsonConvert.SerializeObject(product);
            var httpResponse = await client.PostAsync(BaseUrl + customerId.ToString(), new StringContent(content, Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot add a todo task");
            }
        }

        //TODO: DELETE instead of POST in BasketService
        public async Task ClearBasket(int customerId)
        {
            
            var httpResponse = await client.DeleteAsync(BaseUrl + "basket/clear/" + customerId.ToString());

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot add a todo task");
            }
        }

        public async Task<List<Order>> GetAllOrders(int customerId)
        {
            var httpResponse = await client.GetAsync(BaseUrl + "payment/" + customerId.ToString());

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var orderList = JsonConvert.DeserializeObject<List<Order>>(content);

            return orderList;
        }

        public async Task RemoveBasketItem(int itemId)
        {
            var httpResponse = await client.PostAsync(BaseUrl + "basket/remove/" + itemId.ToString(), null);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }
        }
    }
}
