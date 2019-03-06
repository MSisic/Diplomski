using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Diplomski.Helper;

namespace Diplomski.Models
{
    public class Student:IEntity
    {
        public int Id { get; set; }
        public Korisnik Korisnik { get; set; }    
        public string RFID { get; set; }
        public string SlikaPath { get; set; }

        public byte[] Slika { get; set; }          
        public int SemestarId { get; set; }
        public Semestar Semestar { get; set; }

    }
}