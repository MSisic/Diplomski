using Diplomski.Areas.ModulEdukatori.Models;
using Diplomski.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Diplomski.Helper;
using Diplomski.Models;

namespace Diplomski.Areas.ModulStudent.Controllers
{
    public class AktivnostiController : Controller
    {
        private MojContext ctx = new MojContext();
  
        public ActionResult Index(int? PredmetId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Student")
                {
                    int StudentId = Autentifikacija.LogiraniKorisnik.Id;
                    PrikaziAktivnostVM Model = new PrikaziAktivnostVM();
                    Model.predmeti = new List<SelectListItem>();
                    Model.predmeti.Add(new SelectListItem { Text = "Svi predmeti", Value = null });
                    Model.predmeti.AddRange(ctx.SlusaPredmet.Where(x => !x.IsPolozen && x.StudentId == StudentId)
                        .Select(x => new SelectListItem
                        {
                            Value = x.PredajePredmet.Predmet.Id.ToString(),
                            Text = x.PredajePredmet.Predmet.Naziv
                        }).ToList());
                    Model.IsAktivna = true;
                    Model.Aktivnosti = ctx.Aktivnosti

                            .Where(x => (x.PredajePredmet.PredmetId == PredmetId || !PredmetId.HasValue)
                            && DateTime.Compare(DateTime.Today, x.Datum) <= 0 && x.IsZavrsena == false)
                            .Select(x => new PrikaziAktivnostVM.AktivnostInfo
                            {
                                Id = x.Id,
                                Datum = x.Datum,
                                Kraj = x.Kraj,
                                Naziv = x.Naziv,
                                Pocetak = x.Pocetak,
                                Predmet = x.PredajePredmet.Predmet.Naziv,
                                VrstaAktivnost = x.VrstaAktivnosti.Naziv
                            }).OrderBy(x => x.Datum).ToList();
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