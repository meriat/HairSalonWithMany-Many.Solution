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
            Stylist foundStylist = Stylist.Find(id);
            return View(foundStylist);
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
