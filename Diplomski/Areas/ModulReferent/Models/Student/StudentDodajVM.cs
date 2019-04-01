using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulReferent.Models.Student
{
    public class StudentDodajVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ime je obavezno polje")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno polje")]
        public string Prezime { get; set; }
        [Required(ErrorMessage = "RFID je obavezno polje")]
        public string RFID { get; set; }

        [StringLength(8, ErrorMessage = "Broj indeksa mora imati 8 znakova", MinimumLength = 8)]
        [Required(ErrorMessage = "Broj indeksa je obavezno polje")]
        public string BrojDosijea { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email je obavezno polje")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Odaberite jednu od ponuđenih opcija")]
        public int SemestarId { get; set; }
        [Required(ErrorMessage = "Lozinka je obavezno polje")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Lozinka mora imati minimalno 8 znakova a maksimalno 24")]
        public string Lozinka { get; set; }
        public string LozinkaHelper{ get; set; }
        public List<SelectListItem> semestriStavke { get; set; }

        public string SlikaPath { get; set; }
        public string SlikaPathHelper { get; set; }
        public string Poruka { get; set; }
     
        public HttpPostedFileBase SlikaFile { get; set; }

      
    }
}