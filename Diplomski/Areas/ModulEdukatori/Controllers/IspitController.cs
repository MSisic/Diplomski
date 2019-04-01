using Diplomski.Areas.ModulEdukatori.Models;
using Diplomski.DAL;
using Diplomski.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Diplomski.Models;

namespace Diplomski.Areas.ModulEdukatori.Controllers
{
    public class IspitController : Controller
    {
        private MojContext ctx = new MojContext();
     
        // GET: ModulEdukatori/Ispit
        public ActionResult Index(int ? PredmetId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    PrikaziAktivnostVM Model = new PrikaziAktivnostVM();
                    Model.predmeti = ctx.PredajePredmet.Where(x => x.EdukatorProfesorId == EdukatorID || x.EdukatorAsistentId == EdukatorID)
                        .Select(x => new SelectListItem
                        {
                            Value = x.Predmet.Id.ToString(),
                            Text = x.Predmet.Naziv
                        }).ToList();
                    Model.IsAktivna = true;
                    Model.Aktivnosti = ctx.Aktivnosti
                            .Where(x => (x.PredajePredmet.PredmetId == PredmetId || !PredmetId.HasValue)
                            && DateTime.Compare(DateTime.Today, x.Datum) <= 0 && x.IsZavrsena == false)
                            .Where(x => x.VrstaAktivnosti.Naziv == "Ispit")
                            .Select(x => new PrikaziAktivnostVM.AktivnostInfo
                            {
                                Id = x.Id,
                                Datum = x.Datum,
                                Kraj = x.Kraj,
                                Naziv = x.Naziv,
                                Pocetak = x.Pocetak,
                                Predmet = x.PredajePredmet.Predmet.Naziv,

                            }).OrderBy(x => x.Datum).ToList();
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
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    DodajAktivnostVM Model = new DodajAktivnostVM();
                    Model.Datumi = ucitajDatume();
                    Model.Datum = DateTime.Today;
                    Model.Pocetak = DateTime.Now.TimeOfDay;
                    Model.Kraj = DateTime.Now.AddMinutes(45).TimeOfDay;
                    Model.PredajePredmet = ucitajPredajePredmete();

                    return View("Dodaj", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
           
        }

        public ActionResult Spremi(DodajAktivnostVM aktivnost)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    if (!ModelState.IsValid)
                    {
                        DodajAktivnostVM Model = new DodajAktivnostVM();
                        Model.Datumi = ucitajDatume();
                        Model.PredajePredmet = ucitajPredajePredmete();
                        return View("Dodaj", Model);
                    }

                    Aktivnost a = new Aktivnost();
                    a.Naziv = aktivnost.Naziv;
                    a.Datum = aktivnost.DatumiId;
                    a.Kraj = aktivnost.Kraj;
                    a.Pocetak = aktivnost.Pocetak;
                    a.VrstaAktivnostiId = ctx.VrsteAktivnosti.Where(x => x.Naziv == "Ispit").FirstOrDefault().Id;
                    a.PredajePredmetId = aktivnost.PredajePredmetId;
                    a.IsAktivirana = false;
                    a.IsZavrsena = false;
                    ctx.Aktivnosti.Add(a);
                    ctx.SaveChanges();

                    List<Student> studenti = ctx.SlusaPredmet.Where(x => x.PredajePredmetId == a.PredajePredmetId && x.IsOdslusan)
                        .Select(x => x.Student).ToList();
                    foreach (Student x in studenti)
                    {
                        AktivnostStudent AS = new AktivnostStudent
                        {
                            AktivnostId = a.Id,
                            StudentId = x.Id,
                            IsPrisustvova = false
                        };
                        ctx.AktivnostStudent.Add(AS);
                        ctx.SaveChanges();

                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
           
        }

        public ActionResult Otkazi(int aktivnostId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    Aktivnost aktivnost = ctx.Aktivnosti.Find(aktivnostId);
                    List<AktivnostStudent> studentAktivnosti = ctx.AktivnostStudent.Where(X => X.AktivnostId == aktivnostId).ToList();
                    foreach (var x in studentAktivnosti)
                    {
                        ctx.AktivnostStudent.Remove(x);
                        ctx.SaveChanges();
                    }
                    ctx.Aktivnosti.Remove(aktivnost);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
            
        }

        public ActionResult AktivirajIspit(int aktivnostId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    Aktivnost a = ctx.Aktivnosti.Find(aktivnostId);
                    if (!a.IsAktivirana)
                    {
                        a.IsAktivirana = true;
                        a.Pocetak = DateTime.Now.TimeOfDay;
                        ctx.SaveChanges();
                    }
                    AktivirajAktivnostVM Model = new AktivirajAktivnostVM();
                    Model.studenti = ctx.AktivnostStudent.Where(x => x.AktivnostId == aktivnostId)
                        .Select(x => new AktivirajAktivnostVM.StudentInfo
                        {
                            Id = x.StudentId,
                            ImePrezime = x.Student.Korisnik.Ime + " " + x.Student.Korisnik.Prezime,
                            IsPrisutan = x.IsPrisustvova,
                            RFID = x.Student.RFID,
                            Slika = x.Student.SlikaPath,
                            VrijemeDolaska = x.VrijemeDolaska,
                            VrijemeOdlaska = x.VrijemeOdlska
                        }).ToList();
                    Model.NazivAktivnosti = ctx.Aktivnosti.Find(aktivnostId).Naziv;
                    Model.AktivnostId = aktivnostId;
                    return View("AktivirajIspit", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
           

        }
        public ActionResult Pretrazi(string rfid, int aktivnostId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    AktivirajAktivnostVM Model = new AktivirajAktivnostVM();

                    Student S = ctx.Studenti.Where(x => x.RFID == rfid).Include(x => x.Korisnik).FirstOrDefault();
                    if (S == null)
                    {
                        Model.NazivAktivnosti = ctx.Aktivnosti.Find(aktivnostId).Naziv;
                        Model.AktivnostId = aktivnostId;
                        Model.SlikaPath = "/Slike/pogresan-rfid.png";
                        Model.ImeStudenta = "RFID NE POSTOJI!";
                        Model.RFID = "";
                    }
                    else
                    {
                        AktivnostStudent AS = ctx.AktivnostStudent.Where(x => x.AktivnostId == aktivnostId && x.StudentId == S.Id).FirstOrDefault();
                        if (AS != null)
                        {
                            AS.IsPrisustvova = true;
                            AS.VrijemeDolaska = DateTime.Now.TimeOfDay;
                            ctx.SaveChanges();
                            Model.SlikaPath = S.SlikaPath;
                            Model.ImeStudenta = S.Korisnik.Ime + " " + S.Korisnik.Prezime;
                        }
                        else
                        {
                            Model.SlikaPath = "/Slike/student_Nema_Prava_Pristupa.png";
                            Model.ImeStudenta = "Student nema pravo pristupa!";
                        }

                    }
                    Model.studenti = ctx.AktivnostStudent.Where(x => x.AktivnostId == aktivnostId)
                      .Select(x => new AktivirajAktivnostVM.StudentInfo
                      {
                          Id = x.StudentId,
                          ImePrezime = x.Student.Korisnik.Ime + " " + x.Student.Korisnik.Prezime,
                          IsPrisutan = x.IsPrisustvova,
                          RFID = x.Student.RFID,
                          Slika = x.Student.SlikaPath,
                          VrijemeDolaska = x.VrijemeDolaska,
                          VrijemeOdlaska = x.VrijemeOdlska
                      }).ToList();
                    Model.NazivAktivnosti = ctx.Aktivnosti.Find(aktivnostId).Naziv;
                    Model.AktivnostId = aktivnostId;

                    Model.RFID = "";
                    return View("AktivirajAktivnost", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
          

        }

        public ActionResult ProsliIspiti(int? PredmetId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    PrikaziAktivnostVM Model = new PrikaziAktivnostVM();
                    Model.predmeti = new List<SelectListItem>();
                    Model.predmeti.Add(new SelectListItem { Value = null, Text = "Svi predmeti" });
                    Model.predmeti.AddRange(ctx.PredajePredmet.Where(x => x.EdukatorProfesorId == EdukatorID || x.EdukatorAsistentId == EdukatorID)
                        .Select(x => new SelectListItem
                        {
                            Value = x.Predmet.Id.ToString(),
                            Text = x.Predmet.Naziv
                        }).ToList());
                    Model.IsAktivna = false;
                    Model.Aktivnosti = ctx.Aktivnosti
                            .Where(x => (x.PredajePredmet.PredmetId == PredmetId || !PredmetId.HasValue)
                            && DateTime.Compare(DateTime.Today, x.Datum) >= 0)
                            .Where(x=>x.VrstaAktivnosti.Naziv=="Ispit")
                              .Where(x => x.Datum.Year - DateTime.Now.Year >= -1)
                            .Select(x => new PrikaziAktivnostVM.AktivnostInfo
                            {
                                Id = x.Id,
                                Datum = x.Datum,
                                Kraj = x.Kraj,
                                Naziv = x.Naziv,
                                Pocetak = x.Pocetak,
                                Predmet = x.PredajePredmet.Predmet.Naziv,
                                VrstaAktivnost = x.VrstaAktivnosti.Naziv
                            }).OrderByDescending(x => x.Datum).ToList();
                    return View("Index", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }

        }


        public ActionResult ZavrsiAktivnost(int aktivnostId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    Aktivnost a = ctx.Aktivnosti.Find(aktivnostId);
                    a.IsZavrsena = true;
                    a.Kraj = DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0));
                    ctx.SaveChanges();
                    List<AktivnostStudent> studentAktivnosti = ctx.AktivnostStudent.Where(X => X.AktivnostId == aktivnostId).ToList();
                    foreach (var x in studentAktivnosti)
                    {
                        if (x.IsPrisustvova && x.VrijemeOdlska == new TimeSpan(00, 00, 00))
                        {
                            x.VrijemeOdlska = a.Kraj;
                            ctx.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
            
        }

        public ActionResult UkloniSaPrisustva(int aktivnostId, int studentId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    AktivnostStudent AS = ctx.AktivnostStudent
                        .Where(x => x.StudentId == studentId && x.AktivnostId == aktivnostId)
                        .Include(x => x.Student)
                        .Include(x => x.Student.Korisnik).FirstOrDefault();
                    AS.VrijemeOdlska = DateTime.Now.TimeOfDay;
                    ctx.SaveChanges();

                    AktivirajAktivnostVM Model = new AktivirajAktivnostVM();
                    Model.studenti = ctx.AktivnostStudent.Where(x => x.AktivnostId == aktivnostId)
                        .Select(x => new AktivirajAktivnostVM.StudentInfo
                        {
                            Id = x.StudentId,
                            ImePrezime = x.Student.Korisnik.Ime + " " + x.Student.Korisnik.Prezime,
                            IsPrisutan = x.IsPrisustvova,
                            RFID = x.Student.RFID,
                            Slika = x.Student.SlikaPath,
                            VrijemeDolaska = x.VrijemeDolaska,
                            VrijemeOdlaska = x.VrijemeOdlska
                        }).ToList();
                    Model.NazivAktivnosti = ctx.Aktivnosti.Find(aktivnostId).Naziv;
                    Model.AktivnostId = aktivnostId;
                    Model.SlikaPath = AS.Student.SlikaPath;
                    Model.ImeStudenta = AS.Student.Korisnik.Ime + " " + AS.Student.Korisnik.Prezime;
                    Model.RFID = "";
                    return View("AktivirajIspit", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
           

        }
        public ActionResult SpisakStudenata(int aktivnostId)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Profesor" || korisnik.Uloga.Naziv == "Asistent")
                {
                    int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
                    SpisakStudenataVM Model = new SpisakStudenataVM();
                    Aktivnost aktivnost = ctx.Aktivnosti
                        .Include(x => x.VrstaAktivnosti).Where(x => x.Id == aktivnostId).FirstOrDefault();
                    Model.Aktivnost = aktivnost.VrstaAktivnosti.Naziv + " :: " + aktivnost.Naziv + " - " + aktivnost.Datum.ToString("dd.MM.yyyy");
                    Model.studenti = ctx.AktivnostStudent.Where(x => x.AktivnostId == aktivnostId)
                        .Select(x => new SpisakStudenataVM.StudentInfo
                        {
                            Id = x.StudentId,
                            Ime = x.Student.Korisnik.Ime,
                            Prezime = x.Student.Korisnik.Prezime,
                            IsPrisustvovao = x.IsPrisustvova,
                            VrijemeDolaska = x.VrijemeDolaska,
                            VrijemeOdlaska = x.VrijemeOdlska
                        }).ToList();
                    return View("SpisakStudenata", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
            
        }

        private List<SelectListItem> ucitajPredajePredmete()
        {
            int EdukatorID = Autentifikacija.LogiraniKorisnik.Id;
            return ctx.PredajePredmet.Where(x => x.EdukatorAsistentId == EdukatorID || x.EdukatorProfesorId == EdukatorID)
                 .Select(x => new SelectListItem
                 {
                     Value = x.Id.ToString(),
                     Text = x.Predmet.Naziv
                 }).ToList();
        }
        private List<SelectListItem> ucitajDatume()
        {
            DateTime Danas = DateTime.Today;
            DateTime Kraj = Danas.AddDays(60);
            int brojac = 0;
            List<SelectListItem> povrat = new List<SelectListItem>();

            for (int i = Danas.DayOfYear; i < Kraj.DayOfYear; i++)
            {
                DateTime temp = Danas;
                temp = temp.AddDays(brojac);
                povrat.Add(new SelectListItem
                {
                    Value = temp.ToString("MM/dd/yyyy"),
                    Text = temp.ToString("dd.MM.yyyy. - dddd")
                });
                brojac++;
            }
            return povrat;
        }
    }
}