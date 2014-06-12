using System.Web.Mvc;

namespace Atom.Main.Routing
{
	public class FusionRoutes : AreaRegistration
	{
		public override string AreaName
		{
			get { return "Fusion"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
			   "Fusion_Incident",                                               // Route name
			   "Fusion/{id}",                                                   // URL with parameters
			   new { controller = "Incident", action = "Details", id = "" },    // Parameter defaults
			   new { id = @"\d+" },
			   new[] { "Atom.Main.Areas.Fusion.Controllers" }
			);

			context.MapRoute(
				"Fusion_Search",                                                // Route name
				"Fusion/{controller}/{action}/{id}",                            // URL with parameters
				new { controller = "Search", action = "Index", id = "" },       // Parameter defaults
				null,
				new[] { "Atom.Main.Areas.Fusion.Controllers" }
			);
		}
	}
}