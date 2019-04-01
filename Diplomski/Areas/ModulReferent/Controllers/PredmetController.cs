using Diplomski.Areas.ModulReferent.Models.Predmet;
using Diplomski.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Diplomski.Models;
using Diplomski.Helper;

namespace Diplomski.Areas.ModulReferent.Controllers
{
    public class PredmetController : Controller
    {
        private MojContext ctx = new MojContext();
        public ActionResult Index(int? semestarId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    PredmetPrikaziVM Model = new PredmetPrikaziVM();
                    Model.predmeti = ctx.PredajePredmet.Where(x => !semestarId.HasValue || x.SemestarId == semestarId)
                        .Include(x => x.Predmet).Include(x => x.Semestar).Select(x => new PredmetPrikaziVM.PredmetInfo
                        {
                            ECTS = x.Predmet.ECTS.ToString(),
                            Id = x.Predmet.Id,
                            Naziv = x.Predmet.Naziv,
                            Oznaka = x.Predmet.Oznaka,
                            Semestar = x.Semestar.Naziv
                        }).ToList();
                    Model.semestriStavke = new List<SelectListItem>();
                    Model.semestriStavke.Add(new SelectListItem { Value = null, Text = "Svi semestri" });
                    Model.semestriStavke.AddRange(ctx.Semestri.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GodinaStudija + " -  " + x.Naziv }).ToList());
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
                    PredmetDodajVM Model = new PredmetDodajVM();
                    Model.semestri = ctx.Semestri.OrderBy(x => x.RedniBroj).Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.GodinaStudija + " - " + x.Naziv
                    }).ToList();

                    Model.profesori = ctx.Edukatori.Where(x => x.Korisnik.Uloga.Naziv == "Profesor").Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Titula + " " + x.Korisnik.Ime + " " + x.Korisnik.Prezime
                    }).ToList();

                    Model.asistenti = ctx.Edukatori.Where(x => x.Korisnik.Uloga.Naziv == "Asistent").Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Titula + " " + x.Korisnik.Ime + " " + x.Korisnik.Prezime
                    }).ToList();
                    return View("Dodaj", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
            
        }

        public ActionResult Uredi(int predmetId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    PredmetDodajVM Model = new PredmetDodajVM();
                    PredajePredmet P = ctx.PredajePredmet.Include(x => x.Predmet).Where(x => x.PredmetId == predmetId).FirstOrDefault();
                    Model.asistentId = P.EdukatorAsistentId;
                    Model.profesorId = P.EdukatorProfesorId;
                    Model.SemestarId = P.SemestarId;
                    Model.Naziv = P.Predmet.Naziv;
                    Model.ECTS = P.Predmet.ECTS;
                    Model.Oznaka = P.Predmet.Oznaka;
                    Model.Id = P.PredmetId;
                    Model.semestri = ctx.Semestri.OrderBy(x => x.RedniBroj).Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.GodinaStudija + " - " + x.Naziv
                    }).ToList();

                    Model.profesori = ctx.Edukatori.Where(x => x.Korisnik.Uloga.Naziv == "Profesor").Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Titula + " " + x.Korisnik.Ime + " " + x.Korisnik.Prezime
                    }).ToList();

                    Model.asistenti = ctx.Edukatori.Where(x => x.Korisnik.Uloga.Naziv == "Asistent").Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Titula + " " + x.Korisnik.Ime + " " + x.Korisnik.Prezime
                    }).ToList();
                    return View("Dodaj", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
          

        }
        public ActionResult Detalji(int id)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    PredmetPrikaziVM Model = new PredmetPrikaziVM();
                    Model.predmet = ctx.PredajePredmet.Include(x => x.Semestar)
                          .Include(x => x.Predmet)
                          .Include(x => x.EdukatorAsistent)
                          .Include(x => x.EdukatorProfesor).Where(x => x.PredmetId == id).Select(x => new PredmetPrikaziVM.PredmetInfo
                          {
                              ECTS = x.Predmet.ECTS.ToString(),
                              Id = x.PredmetId,
                              Naziv = x.Predmet.Naziv,
                              Oznaka = x.Predmet.Oznaka,
                              Semestar = x.Semestar.Naziv + " - " + x.Semestar.GodinaStudija,
                              Profesor = x.EdukatorProfesor.Titula + " " + x.EdukatorProfesor.Korisnik.Ime + " " + x.EdukatorProfesor.Korisnik.Prezime,
                              Asistent = x.EdukatorAsistent.Titula + " " + x.EdukatorAsistent.Korisnik.Ime + " " + x.EdukatorAsistent.Korisnik.Prezime,
                          }).FirstOrDefault();

                    return View("Detalji", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
          
        }
        public ActionResult Spremi(PredmetDodajVM predmet)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    if (!ModelState.IsValid)
                    {
                        predmet.semestri = ctx.Semestri.OrderBy(x => x.RedniBroj).Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.GodinaStudija + " - " + x.Naziv
                        }).ToList();

                        predmet.profesori = ctx.Edukatori.Where(x => x.Korisnik.Uloga.Naziv == "Profesor").Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Titula + " " + x.Korisnik.Ime + " " + x.Korisnik.Prezime
                        }).ToList();

                        predmet.asistenti = ctx.Edukatori.Where(x => x.Korisnik.Uloga.Naziv == "Asistent").Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Titula + " " + x.Korisnik.Ime + " " + x.Korisnik.Prezime
                        }).ToList();
                        return View("Dodaj", predmet);
                    }
                    Predmet P;
                    PredajePredmet PNO;
                    if (predmet.Id == 0)
                    {
                        P = new Predmet();
                        ctx.Predmeti.Add(P);
                        PNO = new PredajePredmet();
                        ctx.PredajePredmet.Add(PNO);
                    }
                    else
                    {
                        P = ctx.Predmeti.Find(predmet.Id);
                        PNO = ctx.PredajePredmet.Where(x => x.PredmetId == P.Id).FirstOrDefault();
                    }
                    P.Naziv = predmet.Naziv;
                    P.ECTS = predmet.ECTS;
                    P.Oznaka = predmet.Oznaka;
                    PNO.PredmetId = P.Id;
                    PNO.EdukatorProfesorId = predmet.profesorId;
                    PNO.EdukatorAsistentId = predmet.asistentId;
                    PNO.SemestarId = predmet.SemestarId;
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
           
        }
    }
}
