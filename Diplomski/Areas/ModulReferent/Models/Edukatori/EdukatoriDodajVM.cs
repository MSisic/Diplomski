using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulReferent.Models.Edukatori
{
    public class EdukatoriDodajVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ime je obavezno polje")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno polje")]
        public string Prezime { get; set; }
        public string Titula { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email je obavezno polje")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Korisničko ime je obavezno polje")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Korisničko ime može imati minimalno 8 znakova a maksimalno 24")]
        public string KorisnickoIme { get; set; }
        [Required(ErrorMessage = "Lozinka je obavezno polje")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Lozinka može imati minimalno 8 znakova a maksimalno 24")]
        public string Lozinka { get; set; }
        public string LozinkaHelper { get; set; }
        public List<SelectListItem> uloge { get; set; }
        public int UlogaId { get; set; }
    }
}