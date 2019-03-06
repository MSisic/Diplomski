using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string LozinkaHash { get; set; }
        public string LozinkaSalt { get; set; }
        public string BrojDosijea { get; set; }
        public int UlogaId { get; set; }
        public Uloga Uloga { get; set; }
        public Student Student { get; set; }
        public NastavnoOsoblje NastavnoOsoblje { get; set; }
    }
}