using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulEdukatori.Models
{
    public class DodajAktivnostVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Naziv je obavezno poslje")]       
        public string Naziv { get; set; }

        [DataType(DataType.Date)]
        //[Range(typeof(DateTime),DateTime.Today.ToString(),DateTime.Today.AddDays(100).ToString(),ErrorMessage ="Termin ne može biti zakazan u prošlosti")]
       public DateTime Datum { get; set; }

        [DataType(DataType.Time)]
        [Range(typeof(TimeSpan),"00:00:00","23:59:59",ErrorMessage ="Neispravan format")]
        public TimeSpan Pocetak { get; set; }

        [DataType(DataType.Time)]
        [Range(typeof(TimeSpan), "00:00:00", "23:59:59", ErrorMessage = "Neispravan format")]
        public TimeSpan Kraj { get; set; }

        public int VrstaAktivnostiId { get; set; }
        public List<SelectListItem> VrstaAktivnosti { get; set; }

        public DateTime DatumiId { get; set; }
        public List<SelectListItem> Datumi { get; set; }
        public int PredajePredmetId { get; set; }

        public List<SelectListItem> PredajePredmet { get; set; }
    }
}