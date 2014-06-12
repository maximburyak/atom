using System.Web.Mvc;

namespace Atom.Main.Controllers
{
	public class ErrorController : BaseController
	{
		public ActionResult Index()
		{
			return View("Error");
		}

		public ActionResult NotFound()
		{
			return View();
		}

		public ActionResult ContentMaxExceeded()
		{
			return View();
		}
	}
}
