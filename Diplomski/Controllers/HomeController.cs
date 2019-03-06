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
            return View(k);

        }
    }
}