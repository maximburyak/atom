using System.Web.Mvc;
using Atom.Main.Areas.Suppliers.Services;
using NHibernate;

namespace Atom.Main.Areas.Suppliers.Controllers
{
	public class HomeController : BaseController
	{
		public HomeController(ISession session)
		{
			_cacheManager = new SuppliersCacheManager(session);
		}

		private readonly SuppliersCacheManager _cacheManager;
		[HttpGet]
		public ActionResult Index(int? page)
		{
			return View();
		}

		public ActionResult ClearCache()
		{
			ViewData["CacheMessage"] = "Cache Cleared.";
			_cacheManager.RemoveCacheItems();
			return View("Index");
		}
	}
}
