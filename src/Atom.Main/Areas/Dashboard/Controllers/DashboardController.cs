using System.Web.Mvc;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Dashboard.Services;
using Atom.Main.Areas.Fusion.Services.Domain;
using Atom.Main.Areas.Fusion.Services.Filters;
using Atom.Main.Filters;
using NHibernate;

namespace Atom.Main.Areas.Dashboard.Controllers
{
	[RedirectToSsl(Order = -1), Authorize(Order = 0)]
	public class DashboardController : Controller
	{
		private readonly ISession _session;
		private readonly SearchService _searchService;

		public DashboardController(ISession session)
		{
			_session = session;
			_searchService = new SearchService(session);
		}

		[ObtainUser]
		public ActionResult Index(string userName)
		{
			var user = GetUser(userName);
			var service = new DashboardServices(_session);
			var viewModel = service.PopulateViewModel();
			//disable drag and drop functions so no CRF can be assigned or re-assigned
			if (user.Department.Description != "IT" && user.Department.Description != "PMO")
			{
				viewModel.Draggable = "";
				viewModel.Droppable = "";
			}
			return View(viewModel);
		}

		[ObtainUser]
		public JsonResult JsonData(string userName)
		{
			var user = GetUser(userName);
			var service = new DashboardServices(_session);
			var viewModel = service.PopulateJsonModel();
			//disable drag and drop functions so no CRF can be assigned or re-assigned
			if (user.Department.Description != "IT" && user.Department.Description != "PMO")
			{
				viewModel.Draggable = "";
				viewModel.Droppable = "";
			}

			return new JsonResult { Data = viewModel, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		private User GetUser(string name)
		{
			return _searchService.GetUser(name);
		}

	}
}
