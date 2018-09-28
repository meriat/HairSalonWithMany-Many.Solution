using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System;
using System.Collections.Generic;

namespace HairSalon.Controllers
{
    public class SpecialtyController : Controller
    {
        [HttpGet("/specialties")]
        public ActionResult Index()
        {
            return View(Specialty.GetAll());
        }

        [HttpPost("/specialties")]
        public ActionResult Add(string name)
        {
            Specialty newSpecialty = new Specialty(name);
            newSpecialty.Save();
            return RedirectToAction("Index");
        }

        [HttpGet("/specialties/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object> {};
            Specialty foundSpecialty = Specialty.Find(id);
            List<Stylist> allStylists = Stylist.GetAll();
            model.Add("specialty", foundSpecialty);
            model.Add("stylists", allStylists);
            return View(model);
        }

        [HttpPost("/specialties/{id}/stylists")]
        public ActionResult Add(int id, string stylist_name)
        {
            Stylist addStylist;

            // int addClientInt = int.Parse(clientId);
            addStylist = new Stylist(stylist_name);
            addStylist.Save();

            Specialty foundSpecialty = Specialty.Find(id);
            foundSpecialty.AddStylist(addStylist);

            return RedirectToAction("Details", new {id = foundSpecialty.Id});
        }

        [HttpGet("/specialties/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            Specialty existingSpecialty = Specialty.Find(id);
            return View(existingSpecialty);
        }
        [HttpPost("/specialties/{id}/update")]
        public ActionResult Update(string newName, int id)
        {
            Specialty foundSpecialty = Specialty.Find(id);
            foundSpecialty.Edit(newName);
            return RedirectToAction("Index");
        }
        [HttpPost("/specialties/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Specialty.DeleteSpecialty(id);
            return RedirectToAction("Index");
        }

        // [HttpGet("/stylists/{stylist_id}/clients/{client_id}/update")]
        // public ActionResult UpdateForm(int stylist_id, int client_id)
        // {
        //     Client newClient = Client.Find(client_id);
        //     return View(newClient);
        // }
        // [HttpPost("/stylists/{stylist_id}/clients/{client_id}")]
        // public ActionResult Update(int stylist_id, int client_id, string newName)
        // {
        //     Client foundClient = Client.Find(client_id);
        //     foundClient.Edit(newName);
        //     return RedirectToAction("Details", "Stylist", new { id = stylist_id });
        // }

        // [HttpPost("/stylists/{stylist_id}/clients/{client_id}/delete")]
        // public ActionResult Delete(int stylist_id, int client_id)
        // {
        // Client.DeleteClient(client_id);
        // return RedirectToAction("Details", "Stylist", new {id=stylist_id});
        // }
    }
}
