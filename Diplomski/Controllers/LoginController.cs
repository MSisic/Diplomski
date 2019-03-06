using Diplomski.DAL;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Diplomski.Helper;


namespace Diplomski.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private MojContext ctx = new MojContext();

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Provjera(string username, string password)
        {
            if (password == "" || username == "")
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                using (MojContext db = new MojContext())
                {
                    Korisnik k = ctx.Korisnici.Include(x => x.NastavnoOsoblje)
                    .Include(x => x.Uloga).Include(x => x.Student).Where(x => x.BrojDosijea == username).FirstOrDefault();
                    if (k == null)
                    {
                        return RedirectToAction("Index", "Login");

                    }
                    else
                    {
                        string hash = LozinkaGenerator.GenerateHash(k.LozinkaSalt, password);
                        if (k.LozinkaHash == hash)
                        {
                            Autentifikacija.LogiraniKorisnik = k;
                            //Korisnik K = Autentifikacija.KorisnikSesija;
                            //if (K.Uloga.Naziv == "Student")
                            //    return RedirectToAction("Index", "Home", new { area = "ModulStudent" });
                            //else if (K.Uloga.Naziv == "Profesor" || K.Uloga.Naziv == "Asistent")
                            //    return RedirectToAction("Index", "Home", new { area = "ModulNastavnoOsoblje" });
                            //else if (K.Uloga.Naziv == "Referent")
                            //    return RedirectToAction("Index", "Home", new { area = "Referent" });
                            //else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }

                        else
                        {
                            return RedirectToAction("Index", "Login");
                        }
                    }


                }
            }


        }

        public ActionResult Logout()
        {
            Autentifikacija.odjava();
            return RedirectToAction("Index", "Login");
        }
    }
}
