using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Models
{
    public class Semestar
    {
        public int Id { get; set; }      
        public string Naziv { get; set; }
        public int RedniBroj { get; set; }
        public string GodinaStudija { get; set; }
    }
}