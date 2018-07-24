using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IOTAmHackCore
{
    public class Helper
    {
        public class RoomAPI
        {
            private string _apiBaseURI = "http://iotamhackcore.azurewebsites.net/";
            public HttpClient InitializeClient()
            {
                var client = new HttpClient();
                //Passing service base url    
                client.BaseAddress = new Uri(_apiBaseURI);

                client.DefaultRequestHeaders.Clear();
                //Define request data format    
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return client;
            }
        }
    }
}
