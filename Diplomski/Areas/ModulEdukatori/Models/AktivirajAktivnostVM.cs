using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Areas.ModulEdukatori.Models
{
    public class AktivirajAktivnostVM
    {
        public class StudentInfo
        {
            public int Id { get; set; }
            public string RFID { get; set; }
            public string Slika { get; set; }
            public string ImePrezime { get; set; }
            public bool IsPrisutan { get; set; }
            public TimeSpan VrijemeDolaska { get; set; }
            public TimeSpan VrijemeOdlaska { get; set; }
          
        }
        public List<StudentInfo> studenti { get; set; }
        public string NazivAktivnosti { get; set; }
        public string ImeStudenta { get; set; }
        public int AktivnostId { get; set; }
        public string RFID { get; set; }
        public string Parametar { get; set; }
        public string SlikaPath { get; set; }
        public bool IsValid { get; set; }
    }
}