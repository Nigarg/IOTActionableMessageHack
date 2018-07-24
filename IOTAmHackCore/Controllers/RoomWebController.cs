using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOTAmHackCore.Models;
using Newtonsoft.Json;

namespace IOTAmHackCore.Controllers
{
    public class RoomWebController : Controller
    {
        Helper.RoomAPI _roomAPI = new Helper.RoomAPI();

        public async Task<IActionResult> Index()
        {
            RoomViewModel dto = new RoomViewModel();

            HttpClient client = _roomAPI.InitializeClient();

            HttpResponseMessage res = await client.GetAsync("api/room");

            //Checking the response is successful or not which is sent using HttpClient    
            if (res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api     
                var result = res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the room list    
                dto.Rooms = JsonConvert.DeserializeObject<List<Room>>(result);

            }

            //returning the room list to view    
            return View(dto);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
    }
}
