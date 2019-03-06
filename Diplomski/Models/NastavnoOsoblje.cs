using Diplomski.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Models
{
    public class NastavnoOsoblje:IEntity
    {
        public int Id { get; set; }
        public Korisnik Korisnik { get; set; }      
        public string Titula { get; set; }
   
    }
}