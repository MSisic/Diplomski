using Diplomski.Areas.ModulReferent.Models.Student;
using Diplomski.DAL;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Diplomski.Helper;
using System.IO;
using System.Drawing;
using System.Data.Entity.Core;


namespace Diplomski.Areas.ModulReferent.Controllers
{
    public class StudentController : Controller
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
                    StudentPrikaziVM Model = new StudentPrikaziVM();

                    Model.studenti = ctx.Studenti.Where(x => !semestarId.HasValue || x.SemestarId == semestarId)
                        .Select(x => new StudentPrikaziVM.StudentInfo
                        {
                            Id = x.Id,
                            Ime = x.Korisnik.Ime,
                            Prezime = x.Korisnik.Prezime,
                            BrojDosijea = x.Korisnik.BrojDosijea,
                            Semestar = x.Semestar.GodinaStudija + " - " + x.Semestar.Naziv,
                            SlikaPath = x.SlikaPath
                        }).ToList();



                    Model.semestriStavke = UcitajSemestre();
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
                    StudentDodajVM Model = new StudentDodajVM();
                    Model.semestriStavke = UcitajSemestre();
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
                    StudentDodajVM Model = new StudentDodajVM();
                    Student S = ctx.Studenti.Where(x => x.Id == id).Include(x => x.Korisnik).FirstOrDefault();
                    Model.semestriStavke = UcitajSemestre(S.Id);
                    Model.SemestarId = S.SemestarId;
                    Model.Ime = S.Korisnik.Ime;
                    Model.Prezime = S.Korisnik.Prezime;
                    Model.RFID = S.RFID;
                    Model.Email = S.Korisnik.Email;
                    Model.BrojDosijea = S.Korisnik.BrojDosijea;
                    Model.SlikaPath = S.SlikaPath;
                    Model.SlikaPathHelper = S.SlikaPath;
                    Model.LozinkaHelper = S.Korisnik.LozinkaHash;


                    return View("Dodaj", Model);
                }
                else
                {
                    return View("ZabranaPristupa", new { @area = "" });
                }
            }
           
        }


       private List<SelectListItem> UcitajSemestre(int? studentId=0)
        {
            List<SelectListItem> semestriStavke = new List<SelectListItem>();
            if (studentId == 0)
            {
                semestriStavke.Add(new SelectListItem { Value = null, Text = "Svi semestri" });
                semestriStavke.AddRange(ctx.Semestri
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GodinaStudija + " -  " + x.Naziv }).ToList());
            }
            else
            {
                Student s = ctx.Studenti.Include(x => x.Semestar).Where(x => x.Id == studentId).FirstOrDefault();
                semestriStavke.AddRange(ctx.Semestri.Where(x=>x.RedniBroj>=s.Semestar.RedniBroj)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GodinaStudija + " -  " + x.Naziv }).ToList());

            }

            return semestriStavke;
        }

        public ActionResult Spremi(StudentDodajVM student)
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik;
            if (korisnik == null)
                return RedirectToAction("Index", "Login", new { area = "" });
            else
            {
                if (korisnik.Uloga.Naziv == "Referent")
                {
                    if (string.IsNullOrEmpty(student.Lozinka) && !string.IsNullOrEmpty(student.LozinkaHelper))
                    {
                        student.Lozinka = student.LozinkaHelper;
                    }
                    else if (!ModelState.IsValid)
                    {
                        student.SlikaPath = student.SlikaPathHelper;
                        student.semestriStavke = UcitajSemestre();
                        return View("Dodaj", student);
                    }
                  
                    Student S;
                    if (student.Id == 0)
                    {
                        S = new Student();
                        S.Korisnik = new Korisnik();
                        ctx.Studenti.Add(S);
                    }
                    else
                    {
                        S = ctx.Studenti.Where(x => x.Id == student.Id).Include(x => x.Korisnik).FirstOrDefault();
                    }
                    if (string.IsNullOrEmpty(student.SlikaPathHelper)&&student.SlikaFile==null)
                    {
                        student.Poruka = "Slika je obavezno polje!";
                        student.semestriStavke = UcitajSemestre();
                        return View("Dodaj", student);
                    }
                    if (student.SlikaFile != null)
                    {
                        string fileName = student.Ime + "-" + student.Prezime + "-" + student.BrojDosijea; ;
                        string extension = Path.GetExtension(student.SlikaFile.FileName);
                        fileName = fileName + extension;
                        student.SlikaPath = "/Slike/" + fileName;
                        fileName = Path.Combine(Server.MapPath("/Slike/"), fileName);
                        student.SlikaFile.SaveAs(fileName);
                        S.SlikaPath = student.SlikaPath;
                    }
           
                  
                    S.Korisnik.Ime = student.Ime;
                    S.Korisnik.Prezime = student.Prezime;
                    S.Korisnik.BrojDosijea = student.BrojDosijea;
                    S.RFID = student.RFID;
                    S.SemestarId = student.SemestarId;
                    if (S.Korisnik.LozinkaHash != student.Lozinka)
                    {
                        S.Korisnik.LozinkaSalt = LozinkaGenerator.GenerateSalt();
                        S.Korisnik.LozinkaHash = LozinkaGenerator.GenerateHash(S.Korisnik.LozinkaSalt, student.Lozinka);
                    }
                   
                    S.Korisnik.Email = student.Email;
                    S.Korisnik.UlogaId = ctx.Uloge.Where(x => x.Naziv == "Student").FirstOrDefault().Id;

                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    string poruka = ExceptionHandler.GetConstraintExceptionMessage(ex);
                        return RedirectToAction("Greska", new { tekst = poruka });
                    }

                    DodjeliPredmete(S.Id, S.SemestarId);
                   
                  
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



        private void DodjeliPredmete(int id, int semestarId)
        {
            SlusaPredmet PK = new SlusaPredmet
            {
                StudentId = id,
                IsOdslusan = false,
                IsPolozen = false,
                PostotakPrisustva = 0,
                BrojSatiPrisustva = 0,
                BrojSatiAktivnosti = 0
            };


            List<PredajePredmet> predmeti = ctx.PredajePredmet.Where(x => x.SemestarId == semestarId).ToList();
            foreach (PredajePredmet P in predmeti)
            {
                PK.PredajePredmetId = P.Id;
                if (!PostojiSlusaPredmet(id, P.PredmetId))
                {
                    ctx.SlusaPredmet.Add(PK);
                    ctx.SaveChanges();
                }

            }
        }
        private bool PostojiSlusaPredmet(int studentId, int predmetId)
        {
            List<SlusaPredmet> lista = ctx.SlusaPredmet.Include(x => x.PredajePredmet).ToList();
            foreach (SlusaPredmet x in lista)
            {
                if (x.PredajePredmet.PredmetId == predmetId && x.StudentId == studentId)
                    return true;
            }
            return false;
        }
    }
}