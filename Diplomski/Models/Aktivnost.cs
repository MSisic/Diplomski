using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Diplomski.Helper;

namespace Diplomski.Models
{
    public class Aktivnost:IEntity
    {
        public int Id { get; set; }       
        public string Naziv { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan Pocetak { get; set; }
        public TimeSpan Kraj { get; set; }
        public bool IsAktivirana { get; set; }
        public bool IsZavrsena { get; set; }
        public int VrstaAktivnostiId { get; set; }
        public VrstaAktivnosti VrstaAktivnosti { get; set; }
        public int PredajePredmetId { get; set; }
        public PredajePredmet PredajePredmet { get; set; }
    }
}