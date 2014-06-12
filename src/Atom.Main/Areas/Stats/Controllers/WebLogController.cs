using System;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using Atom.Areas.Fusion.Domain.Models;
using Atom.Main.Areas.Fusion.Controllers;
using Atom.Main.Areas.Stats.Models.ViewModels;
using Atom.Main.Areas.Stats.Services.Domain;
using BeValued.Data.NHibernate.Mvc;
using BeValued.Utilities.Utilities;
using log4net;
using NHibernate;

namespace Atom.Main.Areas.Stats.Controllers
{
	[Authorize(Roles = "Fusion.IT"), Transaction]
	public class WebLogController : BaseController
	{
		private readonly ISession _session;
		private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private const int DefaultResultSize = 500;

		private readonly WebLogOperationService _weblogOperationService;

		public WebLogController(ISession session)
			: base(session)
		{
			_session = session;
			_weblogOperationService = new WebLogOperationService(ConfigurationManager.AppSettings["WebLogArchiveLocation"], session);
		}

		//
		// GET: /Fusion/WebLog/

		public ActionResult Index()
		{
			ListWebLogViewModel model;

			try
			{
				model = _weblogOperationService.PopulateListWebLogViewModel(new ListWebLogViewModel()
																{
																	ResultsBatchSize = DefaultResultSize
																});
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}

			return View("ShowWebLog", model);
		}

		[HttpPost]
		public ActionResult Index(FormCollection formData)
		{
			ListWebLogViewModel model;

			try
			{
				var fromDateTime = ParseDateString(formData.Get("FromDate"));
				var toDateTime = ParseDateString(formData.Get("ToDate"));
				var sortColumn = formData.Get("SortColumn").Trim();
				var isSortDesc = bool.Parse(formData.Get("IsSortDesc") ?? "true");
				var maxResults = int.Parse(formData.Get("MaxResults"));
				var website = formData.Get("Website").Trim();
				var pathFilter = formData.Get("PathFilter").Trim();
				var accessCountFrom = formData.Get("AccessCountFrom");
				var accessCountTo = formData.Get("AccessCountTo");

				model = _weblogOperationService.PopulateListWebLogViewModel(new ListWebLogViewModel()
																					{
																						FromDate = fromDateTime,
																						ToDate = toDateTime,
																						SortColumn = sortColumn,
																						IsSortDesc = isSortDesc,
																						ResultsBatchSize = maxResults,
																						SelectedWebsite = website,
																						PathFilter = pathFilter,
																						AccessCountFrom = accessCountFrom,
																						AccessCountTo = accessCountTo
																					});
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}
			return PartialView("ListWebLog", model);
		}

		[HttpGet]
		public ActionResult Archive()
		{
			ListArchivedWebFilesViewModel model;
			try
			{
				model = _weblogOperationService.PopulateListArchivedWebFilesViewModel(new ListArchivedWebFilesViewModel()
				{
					SortColumn = PropertyUtil.GetName<ArchivedFile>(x => x.CreatedDate),
					IsSortDesc = true
				});
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}
			return View("ShowArchived", model);
		}

		[HttpPost]
		public ActionResult Archive(ArchivedFile file)			//(string webPath, string filePath, string website)
		{
			bool Success;
			try
			{
				//var Success = _weblogOperationService.ArchiveFile(website, webPath, filePath);
				Success = _weblogOperationService.ArchiveFile(file);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}
			return Json(new { Success });
		}

		[HttpPost]
		public ActionResult ListArchived(FormCollection formData)
		{
			ListArchivedWebFilesViewModel model;
			try
			{
				var sortColumn = formData.Get("SortColumn");
				sortColumn = (sortColumn != null)
								? sortColumn.Trim()
								: PropertyUtil.GetName<ArchivedFile>(x => x.CreatedDate);

				var isSortDesc = bool.Parse(formData.Get("IsSortDesc") ?? "true");
				var website = formData.Get("Website");
				var webpath = formData.Get("Webpath");

				model = _weblogOperationService.PopulateListArchivedWebFilesViewModel(new ListArchivedWebFilesViewModel()
																					{
																						SortColumn = sortColumn,
																						IsSortDesc = isSortDesc,
																						SelectedWebsite = website,
																						PathFilter = webpath
																					});
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}
			return PartialView("ListArchived", model);
		}

		[HttpPost]
		public ActionResult Restore(ArchivedFile file)
		{
			bool Success;
			try
			{
				Success = _weblogOperationService.RestoreFile(file);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}
			return Json(new { Success });
		}

		private DateTime? ParseDateString(string dateString)
		{
			DateTime date = new DateTime();
			DateTime? result = null;

			if (dateString != null && DateTime.TryParse(dateString, out date))
				result = date;

			return result;
		}
	}
}
