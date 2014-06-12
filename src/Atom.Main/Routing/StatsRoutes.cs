using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atom.Main.Routing
{
	public class StatsRoutes: AreaRegistration
	{
		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Stats_Search",                                                // Route name
				"Stats/{controller}/{action}",                            // URL with parameters
				new { controller = "WebLog", action = "Index"},       // Parameter defaults
				null,
				new[] { "Atom.Main.Areas.Stats.Controllers" }
			);
		}

		public override string AreaName
		{
			get { return "Stats"; }
		}
	}
}