using Diplomski.DAL;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Diplomski.Helper
{
    public class Autentifikacija
    {
        public static Korisnik LogiraniKorisnik
        {
            get { return (Korisnik)HttpContext.Current.Session["user"]; }
            set { HttpContext.Current.Session["user"] = value; }
        }
        public static void odjava()
        {
            HttpContext.Current.Session.Remove("user");

        }



    }
    }
