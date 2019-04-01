using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Areas.ModulEdukatori.Models
{
    public class PredmetStudentDetaljiVM
    {
        public class AktivnostiInfo
        {
            public int AktivnostId { get; set; }
            public string Naziv { get; set; }
            public DateTime Datum { get; set; }
            public bool IsPrisustvovao { get; set; }
            public TimeSpan VrijemeDolaska { get; set; }
            public TimeSpan VrijemeOdlaska { get; set; }
            public double PostotakPrisustva { get; set; }            
            public double TrajanjeAktivnosti { get; set; }
            public double TrajanjePrisustva { get; set; }
        }
        public List<AktivnostiInfo> Aktivnosti { get; set; }
        public string Student { get; set; }
        public string Predmet { get; set; }
        public double UkupanPostotak { get; set; }
    }
}