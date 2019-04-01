using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulStudent.Models
{
    public class StudentPredmetPrikaziVM
    {
        public class PredmetInfo
        {
           public int Id { get; set; }
            public string Predmet { get; set; }
            public string Semestar { get; set; }
            public double BrojSatiAktivnosti { get; set; }
            public double BrojSatiPrisustva { get; set; }
            public double PostotakPrisustva { get; set; }
            public bool IsOdslusan { get; set; }
        }
       
        public List<SelectListItem> Semestri { get; set; }
        public int SemestarId { get; set; }
        public string Edukator { get; set; }
        public int StudentID { get; set; }
        public string Student { get; set; }
        public List<PredmetInfo> NepolozeniPredmeti { get; set; }
        public List<PredmetInfo> PolozeniPredmeti { get; set; }

    }
}