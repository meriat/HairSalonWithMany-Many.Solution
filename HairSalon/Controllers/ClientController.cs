using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System;
using System.Collections.Generic;

namespace HairSalon.Controllers
{
    public class ClientController : Controller
    {
        [HttpPost("/stylists/{stylist_id}/clients")]
        public ActionResult CreateClient(int stylist_id, string client_name)
        {
            Client newClient = new Client(client_name, stylist_id);
            newClient.Save();
            return RedirectToAction("Details", "Stylist", new { id = stylist_id });
        }

        [HttpGet("/stylists/{stylist_id}/clients/{client_id}/update")]
        public ActionResult UpdateForm(int stylist_id, int client_id)
        {
            Client newClient = Client.Find(client_id);
            return View(newClient);
        }
        [HttpPost("/stylists/{stylist_id}/clients/{client_id}")]
        public ActionResult Update(int stylist_id, int client_id, string newName)
        {
            Client foundClient = Client.Find(client_id);
            foundClient.Edit(newName);
            return RedirectToAction("Details", "Stylist", new { id = stylist_id });
        }

        [HttpPost("/stylists/{stylist_id}/clients/{client_id}/delete")]
        public ActionResult Delete(int stylist_id, int client_id)
        {
        Client.DeleteClient(client_id);
        return RedirectToAction("Details", "Stylist", new {id=stylist_id});
        }
    }
}
