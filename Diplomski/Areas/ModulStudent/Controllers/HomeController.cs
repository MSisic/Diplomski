using Diplomski.Helper;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulStudent.Controllers
{
    public class HomeController : Controller
    {
        // GET: ModulStudent/Home
        public ActionResult Index()
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Student")
                {
                    return RedirectToAction("Index", "Aktivnosti");
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
           
        }
    }
}