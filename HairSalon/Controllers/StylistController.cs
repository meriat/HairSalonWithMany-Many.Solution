using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System;
using System.Collections.Generic;

namespace HairSalon.Controllers
{
    public class StylistController : Controller
    {
        [HttpGet("/stylists")]
        public ActionResult Index()
        {
            List<Stylist> allStylists = Stylist.GetAll();
            return View("Index", allStylists);
        }

        [HttpPost("/stylists")]
        public ActionResult Add(string name)
        {
            Stylist newStylist = new Stylist(name);
            newStylist.Save();
            return RedirectToAction("Index");
        }

        [HttpGet("/stylists/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object> {};
            Stylist foundStylist = Stylist.Find(id);
            List<Client> allClients = Client.GetAll();
            model.Add("stylist", foundStylist);
            model.Add("clients", allClients);
            return View(model);
        }

        

        [HttpPost("/stylists/{id}/clients")]
        public ActionResult Add(int id, string client_name)
        {
            Client addClient;

            // int addClientInt = int.Parse(clientId);
            addClient = new Client(client_name);
            addClient.Save();

            Stylist foundStylist = Stylist.Find(id);
            foundStylist.AddClient(addClient);

            return RedirectToAction("Details", new {id = foundStylist.Id});
        }

        [HttpGet("/stylists/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Stylist existingStylist = Stylist.Find(id);
            return View(existingStylist);
        }
        [HttpPost("/stylists/{id}/update")]
        public ActionResult Update(string newName, int id)
        {
            Stylist foundStylist = Stylist.Find(id);
            foundStylist.Edit(newName);
            return RedirectToAction("Index");
        }
        [HttpPost("/stylists/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Stylist.DeleteStylist(id);
            return RedirectToAction("Index");
        }
    }
}
