using Diplomski.Areas.ModulStudent.Models;
using Diplomski.DAL;
using Diplomski.Helper;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Diplomski.Areas.ModulStudent.Controllers
{
    public class PredmetController : Controller
    {
        private MojContext ctx = new MojContext();
      
      
        public ActionResult Index(int? semestarId,int? studentID)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Student" || korisnik.Uloga.Naziv == "Referent")
                {
                    int StudentId = Autentifikacija.LogiraniKorisnik.Id;
                    if (studentID != null)
                    {
                        StudentId =(int) studentID;
                    }


                    StudentPredmetPrikaziVM Model = new StudentPredmetPrikaziVM();
                    Model.StudentID = StudentId;
                    Student S = ctx.Studenti.Where(x => x.Id == StudentId).Include(x => x.Korisnik).FirstOrDefault();
                    Model.Student =S.Korisnik.Ime + " " + S.Korisnik.Prezime;
                    Model.Semestri = new List<SelectListItem>();
                    Model.Semestri.Add(new SelectListItem { Value = null, Text = "Svi semestri" });
                    Model.Semestri.AddRange(ctx.Semestri.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GodinaStudija + " -  " + x.Naziv }).ToList());

                    Model.NepolozeniPredmeti = ctx.SlusaPredmet
                        .Where(x => !semestarId.HasValue || x.PredajePredmet.SemestarId == semestarId)
                        .Where(x => x.StudentId == StudentId && !x.IsPolozen)
                        .Select(x => new StudentPredmetPrikaziVM.PredmetInfo
                        {
                            Id = x.PredajePredmetId,
                            Predmet = x.PredajePredmet.Predmet.Oznaka + " - " + x.PredajePredmet.Predmet.Naziv,
                            IsOdslusan = x.IsOdslusan,
                            PostotakPrisustva = x.PostotakPrisustva,
                            Semestar = x.PredajePredmet.Semestar.GodinaStudija + " - " + x.PredajePredmet.Semestar.Naziv
                        }).ToList();
                    Model.PolozeniPredmeti = ctx.SlusaPredmet
                        .Where(x => !semestarId.HasValue || x.PredajePredmet.SemestarId == semestarId)
                        .Where(x => x.StudentId == StudentId && x.IsPolozen)
                        .Select(x => new StudentPredmetPrikaziVM.PredmetInfo
                        {
                            Id = x.PredajePredmetId,
                            Predmet = x.PredajePredmet.Predmet.Oznaka + " - " + x.PredajePredmet.Predmet.Naziv,
                            IsOdslusan = x.IsOdslusan,
                            PostotakPrisustva = x.PostotakPrisustva,
                            Semestar = x.PredajePredmet.Semestar.GodinaStudija + " - " + x.PredajePredmet.Semestar.Naziv
                        }).ToList();
                    return View("Index", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }

            
        }
    }
}