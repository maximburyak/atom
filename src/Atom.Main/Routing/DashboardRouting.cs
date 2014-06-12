using System.Web.Mvc;

namespace Atom.Main.Routing
{
    public class DashboardRouting : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Dashboard"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "Dashboard",                                               // Route name
               "Dashboard/{action}/{id}",                                                   // URL with parameters
               new { controller = "Dashboard", action = "Index", id = "" },  // Parameter defaults
               null,
               new[] { "Atom.Main.Areas.Dashboard.Controllers" }
            );
        }
    }
}