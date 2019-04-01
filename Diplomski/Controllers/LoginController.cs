using Diplomski.DAL;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Diplomski.Helper;
using Diplomski.Models.ViewModels;

namespace Diplomski.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private MojContext ctx = new MojContext();

        public ActionResult Index(LoginVM Model=null)
        {
            if (Model == null)
            {
                Model = new LoginVM();
                Model.Lozinka = "";
                Model.BrojDosijea = "";
            }
            
            
            return View("Index",Model);
        }

        public ActionResult Provjera(LoginVM Model)
        {
            if (String.IsNullOrEmpty(Model.BrojDosijea)&& String.IsNullOrEmpty(Model.Lozinka))
            {
                Model.Poruka = "Unesite podatke za prijavu";
                return RedirectToAction("Index", "Login", Model);
            }
            else if (String.IsNullOrEmpty(Model.BrojDosijea))
            {
                Model.Poruka = "Unesite broj dosijea";
                return RedirectToAction("Index", "Login", Model);
            }
            else if (String.IsNullOrEmpty(Model.Lozinka))
            {
                Model.Poruka = "Unesite lozinku";
                return RedirectToAction("Index", "Login", Model);
            }
            else
            {
                using (MojContext db = new MojContext())
                {
                    Korisnik k = ctx.Korisnici.Include(x => x.Edukator)
                    .Include(x => x.Uloga).Include(x => x.Student).Where(x => x.BrojDosijea == Model.BrojDosijea).FirstOrDefault();
                    if (k == null)
                    {
                    Model.Poruka = "Pogrešan broj dosijea!";
                        return RedirectToAction("Index", "Login", Model);

                    }
                    else
                    {
                        string hash = LozinkaGenerator.GenerateHash(k.LozinkaSalt, Model.Lozinka);
                        if (k.LozinkaHash == hash)
                        {
                            Autentifikacija.LogiraniKorisnik = k;                           
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }

                        else
                    {
                        Model.Poruka = "Pogrešan lozinka!";

                        return RedirectToAction("Index", "Login",Model);
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
