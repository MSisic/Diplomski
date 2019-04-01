using Diplomski.DAL;
using Diplomski.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomski.Helper
{
    public class UpdatePrisustvo
    {
        
        public static void UpdatePostotakPrisustva()
        {
            MojContext ctx = new MojContext();
            List<SlusaPredmet> studenti = ctx.SlusaPredmet.Where(x => x.IsPolozen == false).ToList();
            foreach (var x in studenti)
            {
                if (x.BrojSatiPrisustva == 0)
                {
                    x.PostotakPrisustva = 0;
                    ctx.SaveChanges();
                }
                else
                {
                    x.PostotakPrisustva = (x.BrojSatiPrisustva / x.BrojSatiAktivnosti * 100);
                    x.PostotakPrisustva = Math.Round(x.PostotakPrisustva, 2);
                    ctx.SaveChanges();
                }
            
            }
        }
    }
}