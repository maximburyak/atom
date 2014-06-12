using System.Web.Mvc;

namespace Atom.Main
{
	public class BeValuedMvcViewEngine : WebFormViewEngine
	{
		public BeValuedMvcViewEngine()
		{
			MasterLocationFormats = new[] {
				"~/Views/{1}/{0}.master",
				"~/Views/Shared/{0}.master"
			};

			AreaMasterLocationFormats = new[] {
				"~/Areas/{2}/Views/{1}/{0}.master",
				"~/Areas/{2}/Views/Shared/{0}.master"
			};

			ViewLocationFormats = new[] {
				"~/Views/{1}/{0}.aspx",
				"~/Views/Shared/{0}.aspx"
			};

			AreaViewLocationFormats = new[] {
				"~/Areas/{2}/Views/{1}/{0}.aspx",
				"~/Areas/{2}/Views/Shared/{0}.aspx"
			};

			AreaPartialViewLocationFormats = new[] {
				"~/Areas/{2}/Views/{1}/{0}.ascx",
				"~/Areas/{2}/Views/Shared/{0}.ascx",
				"~/Areas/{2}/Views/{1}/Partials/{0}.ascx",
				"~/Areas/{2}/Views/Shared/Partials/{0}.ascx"
			};

			PartialViewLocationFormats = new[] {
				"~/Views/{1}/{0}.ascx",
				"~/Views/Shared/{0}.ascx"
			};
		}

		#region IViewEngine Members

		public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
		{
			return base.FindPartialView(controllerContext, partialViewName, useCache);
		}

		public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
		{
			return base.FindView(controllerContext, viewName, masterName, useCache);
		}

		public override void ReleaseView(ControllerContext controllerContext, IView view)
		{
			base.ReleaseView(controllerContext, view);
		}

		protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
		{
			return base.FileExists(controllerContext, virtualPath);
		}

		#endregion
	}
}
