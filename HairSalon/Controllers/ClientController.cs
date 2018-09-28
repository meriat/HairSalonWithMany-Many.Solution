using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System;
using System.Collections.Generic;

namespace HairSalon.Controllers
{
    public class ClientController : Controller
    {
        [HttpGet("/clients")]
        public ActionResult Index()
        {
            return View(Client.GetAll());
        }

        [HttpPost("/clients")]
        public ActionResult Add(string name)
        {
            Client newClient = new Client(name);
            newClient.Save();
            return RedirectToAction("Index");
        }

        [HttpGet("/clients/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object> {};
            Client foundClient = Client.Find(id);
            List<Stylist> allStylists = Stylist.GetAll();
            model.Add("client", foundClient);
            model.Add("stylists", allStylists);
            return View(model);
        }

        [HttpPost("/clients/{id}/stylists")]
        public ActionResult Add(int id, string stylist_name)
        {
            Stylist addStylist;

            // int addClientInt = int.Parse(clientId);
            addStylist = new Stylist(stylist_name);
            addStylist.Save();

            Client foundClient = Client.Find(id);
            foundClient.AddStylist(addStylist);

            return RedirectToAction("Details", new {id = foundClient.Id});
        }

        [HttpGet("/clients/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Client existingClient = Client.Find(id);
            return View(existingClient);
        }
        [HttpPost("/clients/{id}/update")]
        public ActionResult Update(string newName, int id)
        {
            Client foundClient = Client.Find(id);
            foundClient.Edit(newName);
            return RedirectToAction("Index");
        }
        [HttpPost("/clients/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Client.DeleteClient(id);
            return RedirectToAction("Index");
        }
    }
}
