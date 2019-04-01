using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Diplomski.Models.ViewModels
{
    public class KorisnikIzmjeniVM
    {
        public int Id { get; set; }
       
        public string Ime { get; set; }   
        public string Prezime { get; set; }        
        public string RFID { get; set; }        
        public string BrojDosijea { get; set; }
        public string Titula { get; set; }
        public string Semestar { get; set; }
        public string Uloga { get; set; }

        public bool IsStudent { get; set; }
        public bool IsEdukator { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email je obavezno polje")]
        public string Email { get; set; }
       

        [Required(ErrorMessage = "Lozinka je obavezno polje")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Lozinka mora imati minimalno 8 znakova a maksimalno 24")]
        public string Lozinka { get; set; }      

        public string SlikaPath { get; set; }
        public HttpPostedFileBase SlikaFile { get; set; }
    }
}