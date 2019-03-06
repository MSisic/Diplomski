using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Diplomski.Helper;

namespace Diplomski.Models
{
    public class Predmet:IEntity
    {
        public int Id { get; set; }
    
        public string Naziv { get; set; }
        public string Oznaka { get; set; }
        public int ECTS { get; set; }
        //public int SemestarId { get; set; }
        //public Semestar Semestar { get; set; }
    }
}