using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Areas.ModulEdukatori.Models
{
    public class PrikaziSpisakStudenataVM
    {
        public class PredmetiInfo
        {
            public int id { get; set; }
            public string student { get; set; }
            public double BrojSatiAktivnosti { get; set; }
            public double BrojSatiPrisustva { get; set; }
            public double PostotakPrisustva { get; set; }
            public bool IsOdslusan { get; set; }
        }
        public string Predmet { get; set; }
        public string Edukator { get; set; }
        public int PredmetId { get; set; }
        public List<PredmetiInfo> predmeti { get; set; }
    }
}