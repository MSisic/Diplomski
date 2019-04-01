using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulEdukatori.Models
{
    public class PrikaziAktivnostVM
    {
        public class AktivnostInfo
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public DateTime Datum { get; set; }
            public TimeSpan Pocetak { get; set; }
            public TimeSpan Kraj { get; set; }
            public string VrstaAktivnost { get; set; }
            public string Predmet { get; set; }
        }
        public bool IsAktivna { get; set; }
        public List<AktivnostInfo> Aktivnosti { get; set; }
        public int PredmetId { get; set; }
        public List<SelectListItem> predmeti { get; set; }
    }
}