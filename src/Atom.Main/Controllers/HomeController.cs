using System.Web.Mvc;

namespace Atom.Main.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
