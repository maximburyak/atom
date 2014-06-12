using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Atom.Main.Setup;
using BeValued.Mvc;
using BeValued.Mvc.Areas.Main.Filters.ErrorFilters;
using BeValued.Mvc.Filters;
using StructureMap;

namespace Atom.Main
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{resource}.svc/{*pathInfo}");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?\w*favicon.ico(/.*)?" });
			routes.IgnoreRoute("Content/{*pathInfo}");


			AreaRegistration.RegisterAllAreas();

			routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "", area = "Default" },  // Parameter defaults
				null,
				new[] { "Atom.Main.Controllers" }
			);

			routes.MapRoute(
			"Error",
			"{*url}",
			new { controller = "Error", action = "NotFound", area = "Default" },
			null,
			new[] { "Atom.Main.Controllers" });
		}

		protected void Application_Start()
		{
#if (DEBUG)
			HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
#endif
			log4net.Config.XmlConfigurator.Configure();
			Bootstrapper.Bootstrap();
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new BeValuedMvcViewEngine());
			RegisterRoutes(RouteTable.Routes);
			var container = ObjectFactory.Container;
			DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
			var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
			FilterProviders.Providers.Remove(oldProvider);
			FilterProviders.Providers.Add(new StructureMapFilterAttributeFilterProvider(container));
			RegisterGlobalFilters(GlobalFilters.Filters);
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			bool friendlyErrors = false;
#if (!DEBUG)
			friendlyErrors=true;
#endif
			filters.Add(new ElmahHandledErrorLoggerFilter());

			filters.Add(new DefaultHandleErrorAttribute { ShowFriendlyErrors = friendlyErrors });
		}
		protected void Application_EndRequest()
		{
			// Make sure to dispose of NHibernate session if created on this web request
			ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
		}
	}
}