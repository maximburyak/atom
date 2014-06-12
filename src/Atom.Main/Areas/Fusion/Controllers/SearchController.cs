using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atom.Areas.Fusion.Data.Queries;
using Atom.Areas.Fusion.Data.Queries.Search.TokenManagers;
using Atom.Areas.Fusion.Data.Queries.Search.Tokens;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Models.ViewModels;
using Atom.Main.Areas.Fusion.Services;
using Atom.Main.Areas.Fusion.Services.Domain;
using Atom.Main.Areas.Fusion.Services.Filters;
using BeValued.Data.NHibernate.Mvc;
using NHibernate;
using Filter = Atom.Areas.Fusion.Domain.Models.Filter;


namespace Atom.Main.Areas.Fusion.Controllers
{
	public class SearchController : BaseController
	{
		private readonly ISession _session;
		private readonly SearchService _searchService;
		private readonly FusionCacheManager _cacheManager;
		private readonly SessionStore _sessionStore;

		public SearchController(ISession session)
			: base(session)
		{
			_session = session;
			_searchService = new SearchService(session);
			_cacheManager = new FusionCacheManager(session);
			_sessionStore = new SessionStore();
		}

		[ObtainUser]
		public ActionResult Index(string search, string userName, string additionalinfo, string systemid)
		{
			var user = _searchService.GetUser(userName);
			var model = new SearchViewModel(search, "Query") { search = search ?? "", action = "Query" };

			if (Request.IsAuthenticated)
			{
				if (_sessionStore.Get<Filter>("LastFilter") == null)
					_sessionStore.Store<Filter>("LastFilter", _searchService.DefaultFilter(userName));

				model.filters = _searchService.Filters(userName);
				model.User = user;
				model.DefaultFilter = _sessionStore.Get<Filter>("LastFilter");
			}

			return View("search", model);
		}

		[ObtainUser]
		public ActionResult SearchLinks(string linksearch, int workitemidtolinkto, string userName, string additionalinfo, string systemid)
		{
			var user = _searchService.GetUser(userName);
			var workitem = new ListSingleWorkItem { Id = workitemidtolinkto }.GetQuery(_session).List<WorkItem>().FirstOrDefault();
			var model = new SearchViewModel(linksearch, "LinkableWorkItems")
							{
								User = user,
								search = linksearch ?? "",
								action = "LinkableWorkItems",
								filters = _searchService.Filters(userName),
								DefaultFilter = _searchService.DefaultFilter(userName),
								workItemIdToLinkTo = workitemidtolinkto,
								WorkItemType = workitem.WorkItemType
							};
			return PartialView("searchlinks", model);
		}

		[OutputCache(Duration = 3600, VaryByParam = "", VaryByHeader = "")]
		public JsonResult SearchAutoComplete()
		{
			var m = new PrefixTokenManager();
			var items = new List<SearchExample>();
			m.SearchTokens.ForEach(x => items.AddRange(x.Examples()));
			return Json(items);
		}

		[HttpPost]
		public ActionResult Query(string search, int maxcase)
		{
			var json = _searchService.Query(Server.UrlDecode(search ?? ""), maxcase, User.Identity.Name);
			return Json(json);
		}

		[HttpPost]
		public ActionResult LinkableWorkItems(string search, int maxcase, int currentWorkItemId)
		{
			var json = _searchService.LinkableWorkItems(Server.UrlDecode(search ?? ""), maxcase, User.Identity.Name, currentWorkItemId);
			return Json(json);
		}

		[HttpGet, Authorize, AuthorizeRole(Roles = "Fusion.IT")]
		public ActionResult RefreshCache()
		{
			_cacheManager.RemoveCacheItems();
			return RedirectToAction("Index", new { search = "", area = "Fusion" });
		}

		#region QueryActionResultsByHTTPGet

		[HttpGet, Authorize, ObtainUser]
		public ActionResult MyWork(string userName)
		{
			var user = _searchService.GetUser(userName);
			var model = new SearchViewModel { search = "", action = "MyWork" };

			if (Request.IsAuthenticated)
			{
				if (_sessionStore.Get<Filter>("LastFilter") == null)
					_sessionStore.Store<Filter>("LastFilter", _searchService.DefaultFilter(userName));

				model.filters = _searchService.Filters(userName);
				model.User = user;
				model.DefaultFilter = _sessionStore.Get<Filter>("LastFilter");
			}

			return View("search", model);
		}

		[HttpGet, Authorize, ObtainUser]
		public ActionResult MyIncidents(string userName)
		{
			var user = _searchService.GetUser(userName);
			var model = new SearchViewModel { search = "", action = "MyIncidents" };

			if (Request.IsAuthenticated)
			{
				if (_sessionStore.Get<Filter>("LastFilter") == null)
					_sessionStore.Store<Filter>("LastFilter", _searchService.DefaultFilter(userName));

				model.filters = _searchService.Filters(userName);
				model.User = user;
				model.DefaultFilter = _sessionStore.Get<Filter>("LastFilter");
			}

			return View("search", model);
		}

		[HttpGet, Authorize, ObtainUser]
		public ActionResult MyCRFs(string userName)
		{
			var user = _searchService.GetUser(userName);
			var model = new SearchViewModel { search = "", action = "MyCRFs" };

			if (Request.IsAuthenticated)
			{
				if (_sessionStore.Get<Filter>("LastFilter") == null)
					_sessionStore.Store<Filter>("LastFilter", _searchService.DefaultFilter(userName));

				model.filters = _searchService.Filters(userName);
				model.User = user;
				model.DefaultFilter = _sessionStore.Get<Filter>("LastFilter");
			}

			return View("search", model);
		}

		[HttpGet, Authorize, ObtainUser]
		public ActionResult MyProjects(string userName)
		{
			var user = _searchService.GetUser(userName);
			var model = new SearchViewModel { search = "", action = "MyProjects" };

			if (Request.IsAuthenticated)
			{
				if (_sessionStore.Get<Filter>("LastFilter") == null)
					_sessionStore.Store<Filter>("LastFilter", _searchService.DefaultFilter(userName));

				model.filters = _searchService.Filters(userName);
				model.User = user;
				model.DefaultFilter = _sessionStore.Get<Filter>("LastFilter");
			}

			return View("search", model);
		}

		[HttpGet, Authorize, ObtainUser]
		public ActionResult OpenIncidents(string userName)
		{
			var user = _searchService.GetUser(userName);
			var model = new SearchViewModel { search = "", action = "OpenIncidents" };

			if (Request.IsAuthenticated)
			{
				if (_sessionStore.Get<Filter>("LastFilter") == null)
					_sessionStore.Store<Filter>("LastFilter", _searchService.DefaultFilter(userName));

				model.filters = _searchService.Filters(userName);
				model.User = user;
				model.DefaultFilter = _sessionStore.Get<Filter>("LastFilter");
			}

			return View("search", model);
		}

		[HttpGet, Authorize, ObtainUser]
		public ActionResult OpenCrfs(string userName)
		{
			var user = _searchService.GetUser(userName);
			var model = new SearchViewModel { search = "", action = "OpenCrfs" };

			if (Request.IsAuthenticated)
			{
				if (_sessionStore.Get<Filter>("LastFilter") == null)
					_sessionStore.Store<Filter>("LastFilter", _searchService.DefaultFilter(userName));

				model.filters = _searchService.Filters(userName);
				model.User = user;
				model.DefaultFilter = _sessionStore.Get<Filter>("LastFilter");
			}

			return View("search", model);
		}

		[HttpGet, Authorize, ObtainUser]
		public ActionResult OpenProjects(string userName)
		{
			var user = _searchService.GetUser(userName);
			var model = new SearchViewModel { search = "", action = "OpenProjects" };

			if (Request.IsAuthenticated)
			{
				if (_sessionStore.Get<Filter>("LastFilter") == null)
					_sessionStore.Store<Filter>("LastFilter", _searchService.DefaultFilter(userName));

				model.filters = _searchService.Filters(userName);
				model.User = user;
				model.DefaultFilter = _sessionStore.Get<Filter>("LastFilter");
			}

			return View("search", model);
		}


		#endregion

		#region QueryActionResultsByHTTPPost

		[HttpPost, Transaction, Authorize, ObtainUser]
		public JsonResult FilterSave(string searchMnemonic, string searchFilter, string userName)
		{
			bool result;
			try
			{
				result = _searchService.FilterSave(searchMnemonic, searchFilter, userName);
			}
			catch (Exception)
			{
				result = false;
			}
			return Json(result);
		}

		[HttpPost, Authorize, ObtainUser]
		public JsonResult Filters(string userName)
		{
			var results = (from r in _searchService.Filters(userName)
						   select new { r.Id, r.IsDefault, r.FilterValue, DisplayText = r.DisplayText(), r.Description }).ToList();
			return Json(results);
		}

		[HttpPost, Authorize, ObtainUser]
		public void FilterChanged(string filter, string userName)
		{
			var lastFilter = new Filter() { FilterValue = "", Id = 0, Description = "" };

			foreach (Filter f in _searchService.Filters(userName))
			{
				if (f.FilterValue == filter)
				{
					lastFilter = f;
					break;
				}
			}
			_sessionStore.Store("LastFilter", lastFilter);
		}

		[HttpPost, Authorize, ObtainUser]
		public ActionResult MyWork(string search, int maxcase)
		{
			var json = _searchService.MyWork(User.Identity.Name, maxcase);
			return Json(json);
		}

		[HttpPost, Authorize, ObtainUser]
		public ActionResult MyIncidents(string search, int maxcase)
		{
			return Json(_searchService.MyIncidents(User.Identity.Name, maxcase));
		}

		[HttpPost, Authorize, ObtainUser]
		public ActionResult MyCRFs(string search, int maxcase)
		{
			return Json(_searchService.MyCrfs(User.Identity.Name, maxcase));
		}

		[HttpPost, Authorize, ObtainUser]
		public ActionResult MyProjects(string search, int maxcase)
		{
			return Json(_searchService.MyProjects(User.Identity.Name, maxcase));
		}

		[HttpPost, Authorize, ObtainUser]
		public ActionResult OpenIncidents(string search, int maxcase)
		{
			return Json(_searchService.OpenIncidents(User.Identity.Name, maxcase));
		}

		[HttpPost, Authorize, ObtainUser]
		public ActionResult OpenCrfs(string search, int maxcase)
		{
			return Json(_searchService.OpenCrfs(User.Identity.Name, maxcase));
		}

		[HttpPost, Authorize, ObtainUser]
		public ActionResult OpenProjects(string search, int maxcase)
		{
			return Json(_searchService.OpenProjects(User.Identity.Name, maxcase));
		}

		public ActionResult Test()
		{

			return View();
		}


		#endregion
	}
}
