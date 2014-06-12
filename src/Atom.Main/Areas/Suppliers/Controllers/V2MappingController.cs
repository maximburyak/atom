using System.Web.Mvc;
using Atom.Areas.Suppliers.Domain.Models;
using Atom.Main.Areas.Fusion.Services.Filters;
using Atom.Main.Areas.Suppliers.Services;
using BeValued.Data.NHibernate;
using BeValued.Mvc.ActionResults;
using NHibernate;

namespace Atom.Main.Areas.Suppliers.Controllers
{
	public class V2MappingController : BaseController
	{
		private readonly ISession _session;
		private const int defaultPageSize = 20;
		private readonly V2MappingService _mappingService;


		public V2MappingController(ISession session, INHibernateRepository<SupplierCategoryMappingV2> v2MappingRepo, INHibernateRepository<SupplierCategoryMappingV2Powerplay> v2PpMappingRepo)
		{
			_mappingService = new V2MappingService(session, v2MappingRepo, v2PpMappingRepo);
			_session = session;
		}

		[HttpGet]
		public ActionResult Index()
		{
			return RedirectToAction("MissingV2");
		}

		[HttpGet]
		public ActionResult MissingV2()
		{
			var model = _mappingService.MissingV2Model(0, string.Empty, string.Empty, 0, defaultPageSize);
			return View(model);
		}

		public ActionResult MissingV2BySupplier(int? supplierid, int? page, string Level1S, string Level2S, bool? export)
		{
			if (!supplierid.HasValue)
				return RedirectToAction("MissingV2");

			var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var model = _mappingService.MissingV2Model(supplierid.Value, Level1S, Level2S, currentPageIndex, defaultPageSize, true);
			return View(model);
		}


		public ActionResult ExistingV2()
		{
			var model = _mappingService.ExistingV2MappingModel(0, null, string.Empty, string.Empty, 0, defaultPageSize);
			return View(model);
		}
		public ActionResult ExistingV2BySupplier(int? supplierid, int? page, bool? Disabled, string Level1S, string Level2S, bool? export)
		{
			if (!supplierid.HasValue)
				return RedirectToAction("ExistingV2");

			var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var model = _mappingService.ExistingV2MappingModel(supplierid.Value, Disabled, Level1S, Level2S, currentPageIndex, defaultPageSize);

			if (export.HasValue)
				return new CsvResult<SupplierCategoryMappingV2>(model.Items, string.Format("ExistingMappingCategories_V2_{0}.csv", supplierid));

			return View(model);
		}

		[ObtainUser]
		public EmptyResult AmendV2MappingDisabled(SupplierCategoryMappingV2 mapping, string userName)
		{
			_mappingService.AmendV2MappingDisabled(mapping, userName);
			return new EmptyResult();
		}

		public ActionResult ExistingV2PP()
		{
			return View();
		}

		public ActionResult MissingV2PP()
		{
			var model = _mappingService.MissingV2PpModel(string.Empty, string.Empty, string.Empty, 0, defaultPageSize);
			return View(model);
		}

		public ActionResult MissingV2PPBySupplier(int? page, string FormatS, string FormatTypeS, bool? export)
		{
			var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var model = _mappingService.MissingV2PpModel(FormatS, FormatTypeS, string.Empty, currentPageIndex, defaultPageSize, true);
			return View(model);
		}


		[ObtainUser]
		public EmptyResult SaveV2Mapping(SupplierCategoryMappingV2 mapping, string userName)
		{
			_mappingService.NewSupplierCategoryMappingV2(mapping, userName);
			return new EmptyResult();
		}

		[ObtainUser]
		public EmptyResult SaveV2PPMapping(SupplierCategoryMappingV2Powerplay mapping, string userName)
		{
			_mappingService.NewSupplierCategoryMappingV2PP(mapping, userName);
			return new EmptyResult();
		}

	}
}
