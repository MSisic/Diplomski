using Diplomski.DAL;
using Diplomski.Helper;
using Diplomski.Models;
using Diplomski.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;

namespace Diplomski.Controllers
{
    public class KorisnikController : Controller
    {
        private MojContext ctx = new MojContext();

       public ActionResult Izmjeni()
        {
            if (Autentifikacija.LogiraniKorisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            int KorisnikId = Autentifikacija.LogiraniKorisnik.Id;
            KorisnikIzmjeniVM Model = new KorisnikIzmjeniVM();
            Model.IsStudent = false;
            Model.IsEdukator = false;
            Korisnik K = ctx.Korisnici.Include(x => x.Student)
                .Include(x => x.Edukator).Include(x=>x.Uloga).Include(x=>x.Student.Semestar)
                .Where(x => x.Id == KorisnikId).FirstOrDefault();
            Model.Id = K.Id;
            Model.Ime = K.Ime;
            Model.Prezime = K.Prezime;
            Model.Email = K.Email;
            Model.BrojDosijea = K.BrojDosijea;
            Model.Uloga = K.Uloga.Naziv;
            if (K.Student != null)
            {
                Model.IsStudent = true;
                Model.Semestar = K.Student.Semestar.GodinaStudija + " - " + K.Student.Semestar.Naziv;
                Model.RFID = K.Student.RFID;
                Model.SlikaPath = K.Student.SlikaPath;
            }
            if (K.Edukator != null)
            {
                Model.IsEdukator = true;
                Model.Titula = K.Edukator.Titula;
            }
          
            return View("Izmjeni", Model);
        }
        

    }
}