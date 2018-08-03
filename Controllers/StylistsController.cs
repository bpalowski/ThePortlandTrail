using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using MySql.Data.MySqlClient;

namespace HairSalon.Controllers
{
  public class StylistsController : Controller
  {
        [HttpGet("/stys")]
        public ActionResult Index()
        {
            List<Stylist> allStylists = Stylist.GetAll();
            return View(allStylists);
        }
        [HttpGet("/stys/new")]
        public ActionResult CStylist()
        {
            return View();
        }
        [HttpPost("/stys")]
          public ActionResult Create()
          {
            Stylist newStylist = new Stylist (Request.Form["newStylist"]);
            newStylist.Save();
            List<Stylist> allStylists = Stylist.GetAll();
            return View("Index", allStylists);
          }
          [HttpGet("/stys/{id}/update")]
         public ActionResult New(int id)
         {
             Stylist thisStylist = Stylist.Find(id);
             return View(thisStylist);
         }
         [HttpPost("/stys/{id}/update")]
              public ActionResult UpDate(int id)
              {
                Stylist thisStylist = Stylist.Find(id);
                thisStylist.Edit(Request.Form["newStylistName"]);
                return RedirectToAction("Index");
              }
          [HttpGet("/stys/{id}/delete")]
          public ActionResult DStylist(int id)
          {
              Stylist thisStylist = Stylist.Find(id);
              thisStylist.Delete();
              return View("DStylist");
          }
          [HttpPost("/stys/{id}/delete")]
          public ActionResult D(int id)
          {
            return View("DStylist");
          }
          [HttpGet("/stys/{id}/specia")]
      public ActionResult specialty(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();

        Stylist selectedStylist = Stylist.Find(id);
        List<Client> selectedClients = Client.GetClientsById(id);
        List<Speciality> stylistSpecialties = selectedStylist.GetSpecialties();
        List<Speciality> allSpecialties = Speciality.GetAll();

        model.Add("selectedStylist", selectedStylist);
        model.Add("selectedClients", selectedClients);
        model.Add("stylistSpecialties", stylistSpecialties);
        model.Add("allSpecialties", allSpecialties);

        return View(model);
      }
      [HttpPost("/stys/{id}/specia/new")]
      public ActionResult AddSpecialty(int id)
      {
        Stylist stylist = Stylist.Find(id);
        Speciality specialty = Speciality.Find(int.Parse(Request.Form["special-id"]));
        stylist.AddSpecialty(specialty);
        return RedirectToAction("Index");
      }










      }
    }
