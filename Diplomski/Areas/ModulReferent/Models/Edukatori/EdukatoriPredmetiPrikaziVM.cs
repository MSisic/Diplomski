using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Areas.ModulReferent.Models.Edukatori
{
    public class EdukatoriPredmetiPrikaziVM
    {
        public class PredmetiInfo
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public string Oznaka { get; set; }
            public int ECTS { get; set; }
            public string Semestar { get; set; }
            public int BrojStudenta { get; set; }
        }
        public List<PredmetiInfo> predmeti { get; set; }
        public string Edukator { get; set; }
    }
}