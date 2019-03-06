using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Areas.ModulReferent.Models.NastavnoOsoblje
{
    public class NastavnoOsobljePrikaziVM
    {
        public class NastavnoOsobljeInfo
        {
            public int Id { get; set; }
            public string Titula { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string Email { get; set; }
            public string Uloga { get; set; }
        }
        public List<NastavnoOsobljeInfo> nastavnoOsoblje { get; set; }
    }
}