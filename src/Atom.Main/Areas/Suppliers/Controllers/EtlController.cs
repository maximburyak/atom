using System.Web.Mvc;
using Atom.Main.Areas.Suppliers.Services;
using Atom.Main.Services.Filters;
using BeValued.Data.NHibernate.Mvc;
using NHibernate;

namespace Atom.Main.Areas.Suppliers.Controllers
{
	public class EtlController : BaseController
	{
		private readonly ISession _session;
		private readonly EtlService _etlService;

		public EtlController(ISession session)
		{
			_session = session;
			_etlService = new EtlService(_session);
		}

		[HttpGet, Transaction]
		public ActionResult Index()
		{
			var model = _etlService.ViewModel();
			return View(model);
		}

		[HttpGet, Transaction, AuthorizeRole(Roles = "Fusion.IT")]
		public ActionResult Disable(int id)
		{
			_etlService.Disable(id);
			return RedirectToAction("Index");
		}

		[HttpGet, Transaction, AuthorizeRole(Roles = "Fusion.IT")]
		public ActionResult Enable(int id)
		{
			_etlService.Enable(id);
			return RedirectToAction("Index");
		}
	}
}
