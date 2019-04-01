using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Diplomski.Models.ViewModels
{
    public class LoginVM
    {
        public string Poruka { get; set; }
        public string BrojDosijea { get; set; }      
        public string Lozinka { get; set; }
    }
}