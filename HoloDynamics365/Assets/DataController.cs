using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Assets.Models;
using HoloDynamics365.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets
{
    public class DataController
    {
        // Returns a list of all products present in the crm
        public static async Task<List<Product>> getProducts()
        {
            List<Product> products = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://172.31.99.58/HoloDynamicsAPI/api/product"));
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            products = JsonConvert.DeserializeObject<List<Product>>(json);
            return products;
        }

        // Returns a list of all accounts that are present in the marketing list of a given product (based on id)
        public static async Task<List<Account>> getCustomers(string id)
        {
            List<Account> accounts = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://172.31.99.58/HoloDynamicsAPI/api/product/" + id));
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            accounts = JsonConvert.DeserializeObject<List<Account>>(json);
            return accounts;
        }

        // Returns a list of Info based on productId and accountId
        public static async Task<List<Info>> getHoloInfoByIds(string productId, string accountId)
        {
            List<Info> holoInfo = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://172.31.99.58/HoloDynamicsAPI/api/info/" + productId + "/" + accountId));
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            holoInfo = JsonConvert.DeserializeObject<List<Info>>(json);
            return holoInfo;
        }

    }
}
