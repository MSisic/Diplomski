using Diplomski.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Diplomski.Helper;
using Diplomski.Models;

namespace Diplomski.Areas.ModulReferent.Controllers
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
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    return View();
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
        }
    }
}