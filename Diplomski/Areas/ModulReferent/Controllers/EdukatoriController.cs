using Diplomski.Areas.ModulReferent.Models.Edukatori;
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
    public class EdukatoriController : Controller
    {
        private MojContext ctx = new MojContext();

        public ActionResult Index(int? ulogaId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    EdukatoriPrikaziVM Model = new EdukatoriPrikaziVM();
                    Model.Uloge = new List<SelectListItem>();
                    Model.Uloge.Add(new SelectListItem { Value = null, Text = "Sve uloge" });
                    Model.Uloge.AddRange(ctx.Uloge.Where(x => x.Naziv == "Profesor" || x.Naziv == "Asistent")
                        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
                    Model.edukatori = ctx.Edukatori
                        .Where(x => x.Korisnik.Uloga.Naziv == "Profesor" || x.Korisnik.Uloga.Naziv == "Asistent")
                        .Where(x => !ulogaId.HasValue || x.Korisnik.UlogaId == ulogaId)
                        .Select(x => new EdukatoriPrikaziVM.EdukatoriInfo
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
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
          
        }

        public ActionResult Dodaj()
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    EdukatoriDodajVM Model = new EdukatoriDodajVM();
                    Model.uloge = ctx.Uloge.Where(x => x.Naziv == "Profesor" || x.Naziv == "Asistent").Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Naziv
                    }).ToList();

                    return View("Dodaj", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
          
        }
        public ActionResult Uredi(int id)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    EdukatoriDodajVM Model = new EdukatoriDodajVM();
                    Model.uloge = ctx.Uloge.Where(x => x.Naziv == "Profesor" || x.Naziv == "Asistent").Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Naziv
                    }).ToList();

                    Edukator NO = ctx.Edukatori.Include(x => x.Korisnik).Where(x => x.Id == id).FirstOrDefault();
                    Model.Id = id;
                    Model.Email = NO.Korisnik.Email;
                    Model.Ime = NO.Korisnik.Ime;
                    Model.Titula = NO.Titula;
                    Model.Prezime = NO.Korisnik.Prezime;
                    Model.KorisnickoIme = NO.Korisnik.BrojDosijea;
                    Model.UlogaId = NO.Korisnik.UlogaId;
                    Model.LozinkaHelper = NO.Korisnik.LozinkaHash;
                    return View("Dodaj", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
         
        }

        public ActionResult PrikaziPredmete(int id)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    EdukatoriPredmetiPrikaziVM Model = new EdukatoriPredmetiPrikaziVM();

                    Model.predmeti = ctx.PredajePredmet
                         .Where(x => x.EdukatorAsistentId == id || x.EdukatorProfesorId == id)
                         .Select(x => new EdukatoriPredmetiPrikaziVM.PredmetiInfo
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

                   Edukator e= ctx.Edukatori.Include(x => x.Korisnik).Where(x => x.Id == id).FirstOrDefault();
                    Model.Edukator = e.Titula + " " + e.Korisnik.Ime + " " + e.Korisnik.Prezime;
                    return View("PrikaziPredmete", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
          
        }

        public ActionResult Spremi(EdukatoriDodajVM Edukatori)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    if (string.IsNullOrEmpty(Edukatori.Lozinka) && !string.IsNullOrEmpty(Edukatori.LozinkaHelper))
                    {
                        Edukatori.Lozinka = Edukatori.LozinkaHelper;
                    }
                   else if (!ModelState.IsValid)
                    {
                        Edukatori.uloge = ctx.Uloge.Where(x => x.Naziv == "Profesor" || x.Naziv == "Asistent").Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Naziv
                        }).ToList();

                        return View("Dodaj", Edukatori);
                    }
                    Edukator NO;
                    if (Edukatori.Id == 0)
                    {
                        NO = new Edukator();
                        NO.Korisnik = new Korisnik();
                        ctx.Edukatori.Add(NO);
                    }
                    else
                    {
                        NO = ctx.Edukatori.Include(x => x.Korisnik).Where(x => x.Id == Edukatori.Id).FirstOrDefault();
                    }


                    NO.Korisnik.Ime = Edukatori.Ime;
                    NO.Korisnik.Prezime = Edukatori.Prezime;
                    NO.Titula = Edukatori.Titula;
                    if (NO.Korisnik.LozinkaHash!=Edukatori.Lozinka)
                    {
                        NO.Korisnik.LozinkaSalt = LozinkaGenerator.GenerateSalt();
                        NO.Korisnik.LozinkaHash = LozinkaGenerator.GenerateHash(NO.Korisnik.LozinkaSalt, Edukatori.Lozinka);
                    }
                    
                    NO.Korisnik.BrojDosijea = Edukatori.KorisnickoIme;
                    NO.Korisnik.Email = Edukatori.Email;
                    NO.Korisnik.UlogaId = ctx.Uloge.Find(Edukatori.UlogaId).Id;

                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string poruka = ExceptionHandler.GetConstraintExceptionMessage(ex);
                       return RedirectToAction("Greska", new { tekst = poruka });
                    }
               

                    return RedirectToAction("Index");
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
        
        }
        public ActionResult Greska(string tekst)
        {
            ViewData["greska"] = tekst;
            return View("Greska", new { @area = "" });
        }

    }
}