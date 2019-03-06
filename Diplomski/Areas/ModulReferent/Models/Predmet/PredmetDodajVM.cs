using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulReferent.Models.Predmet
{
    public class PredmetDodajVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv je obavezno polje")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Oznaka je obavezno polje")]

        public string Oznaka { get; set; }
        [Required(ErrorMessage = "Ime je obavezno polje")]
        [Range(2, 7)]
        public int ECTS { get; set; }
        public List<SelectListItem> profesori { get; set; }
        public List<SelectListItem> asistenti { get; set; }
        public List<SelectListItem> semestri { get; set; }
        public int SemestarId { get; set; }
        public int profesorId { get; set; }
        public int asistentId { get; set; }
    }
}