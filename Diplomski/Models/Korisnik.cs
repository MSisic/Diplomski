using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Diplomski.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public string LozinkaHash { get; set; }
        public string LozinkaSalt { get; set; }
      [StringLength(50)]
        public string BrojDosijea { get; set; }
        public int UlogaId { get; set; }
        public Uloga Uloga { get; set; }
        public Student Student { get; set; }
        public Edukator Edukator{ get; set; }
    }
}