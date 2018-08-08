using HoloDynamics365.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharpWSA
{
    public class DataManager
    {
        static HttpClient client = new HttpClient();
        static MenuManager menuMan = new MenuManager();

        public static async Task ProductMenuAsync()
        {
            client.BaseAddress = new Uri("http://localhost:51808/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            List<Product> products = new List<Product>();
            products = await getProducts();

            menuMan.CreateProductMenu(products);
        }
        public static async Task<List<Product>> getProducts()
        {
            List<Product> products = null;
            HttpResponseMessage resp = await client.GetAsync("http://localhost:51808/api/product");

            if (resp.IsSuccessStatusCode)
            {
                var jsonString = await resp.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<Product>>(jsonString);
            }
            return products;
        }
    }
}
