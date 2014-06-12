using System.Web.Mvc;
using Atom.Areas.Suppliers.Domain.Models;
using Atom.Main.Areas.Fusion.Services.Filters;
using Atom.Main.Areas.Suppliers.Models;
using Atom.Main.Areas.Suppliers.Services;
using BeValued.Mvc.ActionResults;
using NHibernate;

namespace Atom.Main.Areas.Suppliers.Controllers
{
	public class MappingController : BaseController
	{
		private readonly ISession _session;
		private readonly MappingService _mappingService;
		private const int defaultPageSize = 20;

		public MappingController(ISession session)
		{
			_session = session;
			_mappingService = new MappingService(session);
		}

		[HttpGet]
		public ActionResult Index()
		{
			return RedirectToAction("Missing");
		}

		[HttpGet]
		public ActionResult Missing()
		{
			var model = _mappingService.MissingModel(0, string.Empty, string.Empty, 0, defaultPageSize);
			return View(model);
		}

		[HttpPost, OutputCache(Duration = 60)]
		public ActionResult ClassesForFormatType(string formattype)
		{
			var model = _session.QueryOver<FormatType_Class>()
				.Where(x => x.FormatType.FormatTypeCode == formattype).Fetch(x => x.Class).
				Eager.List();
			return View(model);
		}
		[HttpPost, OutputCache(Duration = 60)]
		public ActionResult FormatTypesForFormat(string format)
		{
			var model = _session.QueryOver<Format_FormatType>()
				.Where(x => x.Format.FormatCode == format).Fetch(x => x.FormatType).
				Eager.List();
			return View(model);
		}


		public ActionResult MissingBySupplier(int? supplierid, int? page, string Level1S, string Level2S, bool? export)
		{
			if (!supplierid.HasValue)
				return RedirectToAction("Index");

			var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var model = _mappingService.MissingModel(supplierid.Value, Level1S, Level2S, currentPageIndex, defaultPageSize);

			if (export.HasValue)
				return new CsvResult<SupplierFeedsLoadItemMapping>(model.Items, string.Format("MissingSupplierCategories_CMS_{0}.csv", supplierid));

			return View(model);
		}


		public ActionResult ExistingBySupplier(int? supplierid, int? page, bool? Disabled, bool? Ignored, string Level1S, string Level2S, bool? export)
		{
			if (!supplierid.HasValue)
				return RedirectToAction("Existing");

			var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var model = _mappingService.ExistingModel(supplierid.Value, Disabled, Ignored, Level1S, Level2S, currentPageIndex, defaultPageSize);

			if (export.HasValue)
				return new CsvResult<SupplierCategoryMappingCms>(model.Items, string.Format("ExistingSupplierCategories_CMS_{0}.csv", supplierid));

			return View(model);
		}

		public ActionResult Existing()
		{
			var model = _mappingService.ExistingModel(0, null, null, string.Empty, string.Empty, 0, defaultPageSize);
			return View(model);
		}


		[ObtainUser]
		public EmptyResult SaveMapping(SupplierCategoryMappingCms mapping, string userName)
		{
			_mappingService.NewEntry(mapping, userName);
			return new EmptyResult();
		}

		[ObtainUser]
		public EmptyResult AmendIgnored(SupplierCategoryMappingCms mapping, string userName)
		{
			_mappingService.AmendIgnored(mapping, userName);
			return new EmptyResult();
		}

		[ObtainUser]
		public EmptyResult AmendDisabled(SupplierCategoryMappingCms mapping, string userName)
		{
			_mappingService.AmendDisabled(mapping, userName);
			return new EmptyResult();
		}
	}
}
