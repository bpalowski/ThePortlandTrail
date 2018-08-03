using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using MySql.Data.MySqlClient;

namespace HairSalon.Controllers
{
  public class SpecialityController : Controller
  {
    [HttpGet("/special")]
    public ActionResult First()
    {
       List<Speciality> allSpecialties = Speciality.GetAll();
      return View(allSpecialties);
    }
    [HttpGet("/special/new")]
    public ActionResult Show()
      {
        return View();
      }
    [HttpPost("/special")]
    public ActionResult Create()
    {
      Speciality newSpeciality = new Speciality(Request.Form["newDescription"]);
      newSpeciality.Save();
      List<Speciality> allSpecialties = Speciality.GetAll();
      return RedirectToAction("First",allSpecialties);
    }
    [HttpGet("/special/{id}/why")]
    public ActionResult Why(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>{};
      Speciality thisSpecialty = Speciality.Find(id);
      List<Stylist> allStylists = thisSpecialty.GetStylist();
      model.Add("stylists", allStylists);
      model.Add("thisSpecialty", thisSpecialty);
      model.Add("specialtyId", id);
      return View(model);;
    }

  }
}
