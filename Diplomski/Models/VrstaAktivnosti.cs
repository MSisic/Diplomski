using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Diplomski.Helper;

namespace Diplomski.Models
{
    public class VrstaAktivnosti:IEntity
    {
        public int Id { get; set; }
     
        public string Naziv { get; set; }
    }
}