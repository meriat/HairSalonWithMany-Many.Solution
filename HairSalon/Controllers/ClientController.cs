using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System;
using System.Collections.Generic;

namespace HairSalon.Controllers
{
    public class ClientController : Controller
    {
        [HttpPost("/stylists/{stylist_id}/clients")]
        public ActionResult CreateRestaurant(int stylist_id, string client_name)
        {
            Client newClient = new Client(client_name, stylist_id);
            newClient.Save();
            return RedirectToAction("Details", "Client", new { id = stylist_id });
        }
    }
}
