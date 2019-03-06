using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Diplomski.Helper;

namespace Diplomski.Models
{
    public class AktivnostStudent:IEntity
    {
        public int Id { get; set; }
       
        public int AktivnostId { get; set; }
        public Aktivnost Aktivnost { get; set; }
        public Student Student { get; set; }
        public int StudentId { get; set; }

        public TimeSpan VrijemeDolaska { get; set; }
        public TimeSpan VrijemeOdlska { get; set; }
        public bool IsPrisustvova { get; set; }
    }
}