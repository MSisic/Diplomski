using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Models
{
    public class PredajePredmet
    {
        public int Id { get; set; }
        public int PredmetId { get; set; }
        public Predmet Predmet { get; set; }
        public NastavnoOsoblje NastavnoOsobljeAsistent { get; set; }
        public int NastavnoOsobljeAsistentId { get; set; }

        public NastavnoOsoblje NastavnoOsobljeProfesor { get; set; }
        public int NastavnoOsobljeProfesorId { get; set; }
        public int SemestarId { get; set;}
        public Semestar Semestar { get; set; }
      
    }
  
}