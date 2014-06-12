using System.Web.Mvc;

namespace Atom.Main.Routing
{
	public class SupplierRoutes : AreaRegistration
	{
		public override string AreaName
		{
			get { return "Suppliers"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Suppliers_Default",                                              // Route name
				"Suppliers/{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "" },  // Parameter defaults
				null,
				new[] { "Atom.Main.Areas.Suppliers.Controllers" }
			);
		}
	}
}