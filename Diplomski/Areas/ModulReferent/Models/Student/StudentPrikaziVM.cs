using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diplomski.Areas.ModulReferent.Models.Student
{
    public class StudentPrikaziVM
    {
        public class StudentInfo
        {
            public int Id { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string BrojDosijea { get; set; }
            public Image Slika { get; set; }
            public string SlikaPath { get; set; }
            public string Telefon { get; set; }
            public string Email { get; set; }
            public string RFID { get; set; }
            public string Semestar { get; set; }
        }
        public List<StudentInfo> studenti { get; set; }
        public List<SelectListItem> semestriStavke { get; set; }
        public int SemestarId { get; set; }
        public StudentInfo student { get; set; }
    }
}