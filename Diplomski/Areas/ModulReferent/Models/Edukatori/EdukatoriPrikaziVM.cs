using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulReferent.Models.Edukatori
{
    public class EdukatoriPrikaziVM
    {
        public class EdukatoriInfo
        {
            public int Id { get; set; }
            public string Titula { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string Email { get; set; }
            public string Uloga { get; set; }
        }
        public int UlogaId { get; set; }
        public List<SelectListItem> Uloge { get; set; }
        public List<EdukatoriInfo> edukatori { get; set; }
    }
}