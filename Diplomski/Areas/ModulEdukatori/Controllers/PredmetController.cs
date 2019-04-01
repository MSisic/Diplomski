using Diplomski.Areas.ModulReferent.Models.Predmet;
using Diplomski.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Diplomski.Helper;
using Diplomski.Areas.ModulEdukatori.Models;
using Diplomski.Models;

namespace Diplomski.Areas.ModulEdukatori.Controllers
{
    public class PredmetController : Controller
    {
        private MojContext ctx = new MojContext();
        // GET: ModulEdukatori/Predmet
      

        public ActionResult Index(int? semestarId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    PredmetPrikaziVM Model = new PredmetPrikaziVM();
                    Edukator E = ctx.Edukatori.Include(x => x.Korisnik).Where(x => x.Id == EdukatorID).FirstOrDefault();
                    Model.Edukator = E.Titula + " " + E.Korisnik.Ime + " " + E.Korisnik.Prezime;
                    Model.predmeti = ctx.PredajePredmet
                        .Where(x => !semestarId.HasValue || x.SemestarId == semestarId)
                        .Where(x => x.EdukatorProfesorId == EdukatorID || x.EdukatorAsistentId == EdukatorID)
                        .Include(x => x.Predmet)
                        .Include(x => x.Semestar
                        ).Select(x => new PredmetPrikaziVM.PredmetInfo
                        {
                            ECTS = x.Predmet.ECTS.ToString(),
                            Id = x.Predmet.Id,
                            Naziv = x.Predmet.Naziv,
                            Oznaka = x.Predmet.Oznaka,
                            Semestar = x.Semestar.Naziv
                        }).ToList();
                    foreach (var x in Model.predmeti)
                    {
                        x.BrojStudenata = ctx.SlusaPredmet.Where(X => X.IsPolozen == false && X.PredajePredmet.PredmetId == x.Id).Count();
                        x.BrojAktivnosti = ctx.Aktivnosti.Where(X => X.PredajePredmet.PredmetId == x.Id).Count();
                    }
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

        public ActionResult ZakljuciPredmet(int predmetId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    List<SlusaPredmet> studenti = ctx.SlusaPredmet.Where(x => x.PredajePredmet.PredmetId == predmetId && !x.IsPolozen).ToList();
                    foreach (var x in studenti)
                    {
                        double postotak = x.BrojSatiPrisustva / x.BrojSatiAktivnosti * 100;
                        if (postotak >= 66)
                        {
                            x.IsOdslusan = true;

                        }
                        else
                        {
                            x.IsOdslusan = false;
                        }
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("SpisakStudenata", new { predmetId = predmetId });
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }           
        }
        public ActionResult DodijeliPrisustvo(int predmetId,int studentId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    SlusaPredmet SP = ctx.SlusaPredmet.Where(x => x.StudentId == studentId && x.PredajePredmet.PredmetId == predmetId).FirstOrDefault();
                    SP.IsOdslusan = true;
                    ctx.SaveChanges();


                    return RedirectToAction("SpisakStudenata", new { predmetId = predmetId });
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
        }
        public ActionResult PolozenPredmet(int predmetId, int studentId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    SlusaPredmet SP = ctx.SlusaPredmet.Where(x => x.StudentId == studentId && x.PredajePredmet.PredmetId == predmetId).FirstOrDefault();
                    SP.IsPolozen = true;
                    ctx.SaveChanges();
                    return RedirectToAction("SpisakStudenata", new { predmetId = predmetId });
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
        }
        public ActionResult DetaljanPrikaz(int predmetId, int studentId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                PredmetStudentDetaljiVM Model = new PredmetStudentDetaljiVM();
                Model.Predmet = ctx.Predmeti.Find(predmetId).Naziv;
                Student S = ctx.Studenti.Include(x => x.Korisnik).Where(x => x.Id == studentId).FirstOrDefault();
                Model.Student = S.Korisnik.Ime + " " + S.Korisnik.Prezime;
                Model.Aktivnosti = ctx.AktivnostStudent
                    .Where(x => x.StudentId == studentId)
                    .Where(x => x.Aktivnost.PredajePredmet.PredmetId == predmetId)
                    .Select(x => new PredmetStudentDetaljiVM.AktivnostiInfo
                    {   AktivnostId  =x.AktivnostId,
                        Naziv = x.Aktivnost.Naziv,
                        Datum = x.Aktivnost.Datum,
                        IsPrisustvovao = x.IsPrisustvova,
                        VrijemeDolaska = x.VrijemeDolaska,
                        VrijemeOdlaska = x.VrijemeOdlska                       
                    }).ToList();
                double ukupnoAktivnosti = 0;
                double ukupnoPrisustva = 0;
                List<Aktivnost> tempAktivnost = ctx.Aktivnosti                   
                    .Where(x => x.PredajePredmet.PredmetId == predmetId).ToList();
                foreach (var x in tempAktivnost)
                {
                    PredmetStudentDetaljiVM.AktivnostiInfo temp= Model.Aktivnosti.Where(a => a.AktivnostId == x.Id).FirstOrDefault();
                    if (temp != null)
                    {
                        temp.TrajanjeAktivnosti = x.Kraj.TotalMinutes - x.Pocetak.TotalMinutes;

                    }

                }
                foreach (var x in Model.Aktivnosti)
                {
                    x.TrajanjePrisustva = x.VrijemeOdlaska.TotalMinutes - x.VrijemeDolaska.TotalMinutes;

                    ukupnoAktivnosti += x.TrajanjeAktivnosti;
                    ukupnoPrisustva += x.TrajanjePrisustva;
                    if (x.TrajanjePrisustva != 0)
                    {
                        x.PostotakPrisustva = x.TrajanjePrisustva / x.TrajanjeAktivnosti * 100;
                        x.PostotakPrisustva = Math.Round(x.PostotakPrisustva, 0);
                    }
                }
                List<SlusaPredmet> predmeti = ctx.SlusaPredmet.Include(x => x.PredajePredmet)
                    .Include(x => x.Student)
                    .Include(x => x.PredajePredmet.Predmet)
                    .Where(x => x.PredajePredmet.PredmetId == predmetId && x.StudentId == studentId).ToList();
                foreach (var x in predmeti)
                {
                    if (x.BrojSatiPrisustva != 0)
                    {
                        Model.UkupanPostotak =Math.Round( x.BrojSatiPrisustva / x.BrojSatiAktivnosti * 100,0);
                                           }
                }
                return View("DetaljanPrikaz", Model);
              
            }
        }
        public ActionResult SpisakStudenata(int predmetId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    PrikaziSpisakStudenataVM Model = new PrikaziSpisakStudenataVM();


                    Model.predmeti = ctx.SlusaPredmet.Include(x => x.PredajePredmet).Include(x => x.Student)
                       .Include(x => x.PredajePredmet.Predmet)
                       .Where(x => x.PredajePredmet.PredmetId == predmetId && x.IsPolozen == false)
                       .Where(x => x.PredajePredmet.EdukatorProfesorId == EdukatorID || x.PredajePredmet.EdukatorAsistentId == EdukatorID)
                       .Select(x => new PrikaziSpisakStudenataVM.PredmetiInfo
                       {        id =(int)x.StudentId,
                           IsOdslusan = x.IsOdslusan,
                           BrojSatiAktivnosti = x.BrojSatiAktivnosti,
                           BrojSatiPrisustva = x.BrojSatiPrisustva,
                           PostotakPrisustva = x.PostotakPrisustva,
                           student = x.Student.Korisnik.Ime + " " + x.Student.Korisnik.Prezime
                       }).ToList();
                    Model.PredmetId = predmetId;
                    foreach (PrikaziSpisakStudenataVM.PredmetiInfo x in Model.predmeti)
                    {
                        if (x.BrojSatiPrisustva != 0)
                        {
                            x.PostotakPrisustva = x.BrojSatiPrisustva / x.BrojSatiAktivnosti * 100;
                            x.PostotakPrisustva = Math.Round(x.PostotakPrisustva, 0);
                        }

                    }
                    Model.Predmet = ctx.Predmeti.Find(predmetId).Naziv;

                    return View("SpisakStudenata", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
          
        }
    }
}