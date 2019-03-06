using Diplomski.Areas.ModulReferent.Models.NastavnoOsoblje;
using Diplomski.DAL;
using Diplomski.Helper;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace Diplomski.Areas.ModulReferent.Controllers
{
    public class NastavnoOsobljeController : Controller
    {
        private MojContext ctx = new MojContext(); public ActionResult Index()
        {
            NastavnoOsobljePrikaziVM Model = new NastavnoOsobljePrikaziVM();

            Model.nastavnoOsoblje = ctx.NastavnoOsoblje
                .Select(x => new NastavnoOsobljePrikaziVM.NastavnoOsobljeInfo
                {
                    Id = x.Id,
                    Email = x.Korisnik.Email,
                    Ime = x.Korisnik.Ime,
                    Prezime = x.Korisnik.Prezime,
                    Titula = x.Titula,
                    Uloga = x.Korisnik.Uloga.Naziv
                }).ToList();

            return View("Index", Model);
        }

        public ActionResult Dodaj()
        {
            NastavnoOsobljeDodajVM Model = new NastavnoOsobljeDodajVM();
            Model.uloge = ctx.Uloge.Where(x => x.Naziv == "Profesor" || x.Naziv == "Asistent").Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Naziv
            }).ToList();

            return View("Dodaj", Model);
        }
        public ActionResult Uredi(int id)
        {
            NastavnoOsobljeDodajVM Model = new NastavnoOsobljeDodajVM();
            Model.uloge = ctx.Uloge.Where(x => x.Naziv == "Profesor" || x.Naziv == "Asistent").Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Naziv
            }).ToList();

            NastavnoOsoblje NO = ctx.NastavnoOsoblje.Include(x => x.Korisnik).Where(x => x.Id == id).FirstOrDefault();
            Model.Id = id;
            Model.Email = NO.Korisnik.Email;
            Model.Ime = NO.Korisnik.Ime;
            Model.Titula = NO.Titula;
            Model.Prezime = NO.Korisnik.Prezime;
            Model.KorisnickoIme = NO.Korisnik.BrojDosijea;
            Model.UlogaId = NO.Korisnik.UlogaId;
            return View("Dodaj", Model);
        }

        public ActionResult PrikaziPredmete(int id)
        {
            NastavnoOsobljePredmetiPrikaziVM Model = new NastavnoOsobljePredmetiPrikaziVM();

            Model.predmeti = ctx.PredajePredmet
                 .Where(x => x.NastavnoOsobljeAsistentId == id || x.NastavnoOsobljeProfesorId == id)
                 .Select(x => new NastavnoOsobljePredmetiPrikaziVM.PredmetiInfo
                 {
                     BrojStudenta = 0,
                     Naziv = x.Predmet.Naziv,
                     ECTS = x.Predmet.ECTS,
                     Id = x.PredmetId,
                     Oznaka = x.Predmet.Oznaka,
                     Semestar = x.Semestar.GodinaStudija + " - " + x.Semestar.Naziv
                 })
                 .ToList();
            foreach (var P in Model.predmeti)
            {
                P.BrojStudenta = ctx.SlusaPredmet.Where(x => x.PredajePredmet.PredmetId == P.Id).Count();
            }

            return View("PrikaziPredmete", Model);
        }

        public ActionResult Spremi(NastavnoOsobljeDodajVM nastavnoOsoblje)
        {
            if (!ModelState.IsValid)
            {
                nastavnoOsoblje.uloge = ctx.Uloge.Where(x => x.Naziv == "Profesor" || x.Naziv == "Asistent").Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Naziv
                }).ToList();
                
                return View("Dodaj", nastavnoOsoblje);
            }
            NastavnoOsoblje NO;
            if (nastavnoOsoblje.Id == 0)
            {
                NO = new NastavnoOsoblje();
                NO.Korisnik = new Korisnik();
                ctx.NastavnoOsoblje.Add(NO);
            }
            else
            {
                NO = ctx.NastavnoOsoblje.Include(x => x.Korisnik).Where(x => x.Id == nastavnoOsoblje.Id).FirstOrDefault();
            }


            NO.Korisnik.Ime = nastavnoOsoblje.Ime;
            NO.Korisnik.Prezime = nastavnoOsoblje.Prezime;
            NO.Titula = nastavnoOsoblje.Titula;
            NO.Korisnik.LozinkaSalt = LozinkaGenerator.GenerateSalt();
            NO.Korisnik.LozinkaHash = LozinkaGenerator.GenerateHash(NO.Korisnik.LozinkaSalt, nastavnoOsoblje.Lozinka);
            NO.Korisnik.BrojDosijea = nastavnoOsoblje.KorisnickoIme;
            NO.Korisnik.Email = nastavnoOsoblje.Email;
            NO.Korisnik.UlogaId = ctx.Uloge.Find(nastavnoOsoblje.UlogaId).Id;

            ctx.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}