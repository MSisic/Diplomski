using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulReferent.Models.Predmet
{
    public class PredmetPrikaziVM
    {
        public class PredmetInfo
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public string Oznaka { get; set; }
            public string ECTS { get; set; }
            public string Semestar { get; set; }
            public string Profesor { get; set; }
            public string Asistent { get; set; }
            public int BrojStudenata { get; set; }
            public int BrojAktivnosti { get; set; }
        }
        public List<PredmetInfo> predmeti { get; set; }
        public List<SelectListItem> semestriStavke { get; set; }
        public PredmetInfo predmet { get; set; }
        public string Edukator { get; set; }
        public int SemestarId { get; set; }
    }
}