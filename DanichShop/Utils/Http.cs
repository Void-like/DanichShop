using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DanichShop.Models;
namespace DanichShop.Utils
{
    public class Http
    {
        static HttpClient client;
        public static HttpClient GetHttpClient()
        {
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("http://37.8.146.204:7081/");
            }

            if (ActiveUser.Token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActiveUser.Token);
            }

            return client;
        }
    }
}
