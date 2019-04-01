using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Areas.ModulEdukatori.Models
{
    public class SpisakStudenataVM
    {
        public class StudentInfo
        {
            public int Id { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public bool IsPrisustvovao { get; set; }
            public TimeSpan VrijemeDolaska { get; set; }
            public TimeSpan VrijemeOdlaska { get; set; }
            public double PostotakPrisustva { get; set; }

        }

        public string Aktivnost { get; set; }
        public List<StudentInfo> studenti { get; set; }
    }
}