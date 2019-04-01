using Diplomski.Helper;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Diplomski.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Korisnik k = Autentifikacija.LogiraniKorisnik;
            if (k == null)
                return RedirectToAction("Index", "Login");

           else if (k.Uloga.Naziv=="Student")
            {
                return RedirectToAction("Index", "Home", new { area = "ModulStudent" });

            }
            else if (k.Uloga.Naziv == "Referent")
            {
                return RedirectToAction("Index", "Home", new { area = "ModulReferent" });

            }
            else if (k.Uloga.Naziv == "Asistent"|| k.Uloga.Naziv == "Profesor")
            {
                return RedirectToAction("Index", "Home",new { area = "ModulEdukatori" });

            }
            else
            {
                return View(k);

            }

        }
    }
}