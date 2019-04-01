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
        public Edukator EdukatorAsistent { get; set; }
        public int EdukatorAsistentId { get; set; }

        public Edukator EdukatorProfesor { get; set; }
        public int EdukatorProfesorId { get; set; }
        public int SemestarId { get; set;}
        public Semestar Semestar { get; set; }
      
    }
  
}