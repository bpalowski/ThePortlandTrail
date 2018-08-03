using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using MySql.Data.MySqlClient;

namespace HairSalon.Controllers
{
  public class ClientsController : Controller
  {
    [HttpGet("/clis")]
    public ActionResult List()
    {
      List<Client> allClients = Client.GetAll();
      return View(allClients);
    }
    [HttpGet("/clis/new")]
    public ActionResult CreateForm()
    {
        return View();
      }
    [HttpPost("/clis")]
    public ActionResult Create()
    {
      Client newClient = new Client(Request.Form["newClient"],int.Parse(Request.Form["StylistId"]));
      newClient.Save();
      List<Client> allClients = Client.GetAll();
      return View("List",allClients);
    }
    [HttpGet("/clis/{id}/update")]
   public ActionResult Up(int id)
   {
       Client thisClient = Client.Find(id);
       return View(thisClient);
   }
   [HttpPost("/clis/{id}/update")]
        public ActionResult UpDate(int id)
        {
          Client thisClient = Client.Find(id);
          thisClient.Edit(Request.Form["newClient"]);
          return RedirectToAction("List");
        }
     [HttpGet("/clis/{id}/delete")]
      public ActionResult No(int id)
      {
          Client thisClient = Client.Find(id);
          thisClient.Delete();
          return View("No");
      }
      [HttpGet("/clis/{id}/info")]
      public ActionResult Info()
      {

        return View();
      }

}
}
