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

namespace Diplomski.Areas.ModulReferent.Controllers
{
    public class StudentController : Controller
    {
        private MojContext ctx = new MojContext();
   
        public ActionResult Index(int? semestarId)
        {
            StudentPrikaziVM Model = new StudentPrikaziVM();

            Model.studenti = ctx.Studenti.Where(x => !semestarId.HasValue || x.SemestarId == semestarId)
                .Select(x => new StudentPrikaziVM.StudentInfo
                {
                    Id = x.Id,
                    Ime = x.Korisnik.Ime,
                    Prezime = x.Korisnik.Prezime,
                    BrojDosijea = x.Korisnik.BrojDosijea,
                    SlikaPath=x.SlikaPath
        }).ToList();

          

            Model.semestriStavke = UcitajSemestre();
            return View("Index", Model);
        }

        public ActionResult Dodaj()
        {
            StudentDodajVM Model = new StudentDodajVM();
            Model.semestriStavke = UcitajSemestre();
            return View("Dodaj", Model);
        }

        private Image byteArrayToImage(byte[] slikaThumb)
        {
            MemoryStream ms = new MemoryStream(slikaThumb);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public ActionResult Uredi(int id)
        {
            StudentDodajVM Model = new StudentDodajVM();
            Student S = ctx.Studenti.Where(x => x.Id == id).Include(x => x.Korisnik).FirstOrDefault();
            Model.semestriStavke = UcitajSemestre();
            Model.SemestarId = S.SemestarId;
            Model.Ime = S.Korisnik.Ime;
            Model.Prezime = S.Korisnik.Prezime;
            Model.RFID = S.RFID;
            Model.Email = S.Korisnik.Email;
            Model.BrojDosijea = S.Korisnik.BrojDosijea;
         


            return View("Dodaj", Model);
        }


        private List<SelectListItem> UcitajSemestre()
        {
            List<SelectListItem> semestriStavke = new List<SelectListItem>();
            semestriStavke.Add(new SelectListItem { Value = null, Text = "Odaberite semestar" });
            semestriStavke.AddRange(ctx.Semestri.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GodinaStudija + " -  " + x.Naziv }).ToList());
            return semestriStavke;
        }

        public ActionResult Spremi(StudentDodajVM student)
        {
            if (!ModelState.IsValid)
            {
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

            string fileName = student.Ime + "-" + student.Prezime + "-" + student.BrojDosijea; ;
            string extension = Path.GetExtension(student.SlikaFile.FileName);
            fileName = fileName + extension;
            student.SlikaPath = "~/Slike/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Slike/"), fileName);
            student.SlikaFile.SaveAs(fileName);         
            Image orginalnaslika = Image.FromFile(fileName);
            Image slika = orginalnaslika;
            MemoryStream ms = new MemoryStream();
            slika.Save(ms, orginalnaslika.RawFormat);
            S.SlikaPath = student.SlikaPath;
            S.Slika = ms.ToArray();
            //  S.SlikaFile.SaveAs(fileName);
            S.Korisnik.Ime = student.Ime;
            S.Korisnik.Prezime = student.Prezime;
            S.Korisnik.BrojDosijea = student.BrojDosijea;
            S.RFID = student.RFID;
            S.SemestarId = student.SemestarId;

            S.Korisnik.LozinkaSalt = LozinkaGenerator.GenerateSalt();
            S.Korisnik.LozinkaHash = LozinkaGenerator.GenerateHash(S.Korisnik.LozinkaSalt, student.Lozinka);
            S.Korisnik.Email = student.Email;
            S.Korisnik.UlogaId = ctx.Uloge.Where(x => x.Naziv == "Student").FirstOrDefault().Id;

            ctx.SaveChanges();
            DodjeliPredmete(S.Id, S.SemestarId);
            return RedirectToAction("Index");
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