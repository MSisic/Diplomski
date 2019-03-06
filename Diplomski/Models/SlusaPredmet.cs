using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Diplomski.Helper;

namespace Diplomski.Models
{
    public class SlusaPredmet:IEntity
    {
        public int Id { get; set; }      
        public int PredajePredmetId { get; set; }
        public PredajePredmet PredajePredmet { get; set; }
        public Student Student { get; set; }
        public int? StudentId { get; set; }
        public bool IsPolozen { get; set; }
        public bool IsOdslusan { get; set; }
        public double BrojSatiPrisustva { get; set; }
        public double BrojSatiAktivnosti { get; set; }
        public double PostotakPrisustva { get; set; }

     
    }
}