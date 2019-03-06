using System.Web.Mvc;

namespace Diplomski.Areas.ModulNastavnoOsoblje
{
    public class ModulNastavnoOsobljeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ModulNastavnoOsoblje";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ModulNastavnoOsoblje_default",
                "ModulNastavnoOsoblje/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}