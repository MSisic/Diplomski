using Diplomski.DAL;
using Diplomski.Helper;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulEdukatori.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    UpdatePrisustvo.UpdatePostotakPrisustva();
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