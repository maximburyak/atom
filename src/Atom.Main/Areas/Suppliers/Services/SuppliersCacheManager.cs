using System.Collections.Generic;
using System.Linq;
using Atom.Areas.Suppliers.Data.Query;
using Atom.Areas.Suppliers.Domain.Models;
using Atom.Main.Areas.Suppliers.Models;
using BeValued.Utilities.MVC.Services;
using NHibernate;

namespace Atom.Main.Areas.Suppliers.Services
{
	public class SuppliersCacheManager
	{
		private readonly ISession _session;
		private readonly SuppliersCacheService _cacheService;

		public SuppliersCacheManager(ISession session)
		{
			_session = session;
			_cacheService = new SuppliersCacheService();
		}

		public IList<SupplierCms> Suppliers()
		{
			var items = _cacheService.Get("suppliers",
							  () => new ListSupplierCms().GetQuery(_session).Future<SupplierCms>());

			return items.OrderBy(x => x.Name).ToList();
		}

		public void RemoveCacheItems()
		{
			_cacheService.RemoveAll();
		}

		public void RemoveCacheId(string cacheId)
		{
			_cacheService.Remove(cacheId);
		}

		public IList<Format> Formats()
		{
			var items = _cacheService.Get("formats",
							  () => new ListFormatCms().GetQuery(_session).Future<Format>());
			return items.Where(x => x != null && !string.IsNullOrEmpty(x.Description)).ToList();
		}

		public IList<SupplierFeedsLoadItemMapping> MissingMappings(int supplierId)
		{
			var items = _cacheService.Get("missingmappings-" + supplierId, () => MissingMappingsList(supplierId));
			return items;
		}

		private IList<SupplierFeedsLoadItemMapping> MissingMappingsList(int supplierId)
		{
			var queriedItems = new ListSupplierFeedLoadItemsWithNoCmsMappings { Id = supplierId }.GetQuery(_session).Future<SupplierFeedsLoadItem>();
			// Then we want to populate the finished model
			var items = (from i in queriedItems
						 select new SupplierFeedsLoadItemMapping { SupplierId = i.SupplierId, Level1 = i.Level1, Level2 = i.Level2, Level3 = i.Level3, Level4 = i.Level4 }).ToList();

			return items;
		}

		public IList<SupplierCategoryMappingCms> ExistingMappings(int supplierId)
		{
			var items = _cacheService.Get("existingmappings-" + supplierId, () => ExistingMappingsList(supplierId));
			return items;
		}

		private IList<SupplierCategoryMappingCms> ExistingMappingsList(int supplierId)
		{
			var items = new ListSupplierCategoryMappingCms { SupplierId = supplierId }.GetQuery(_session).Future<SupplierCategoryMappingCms>().ToList();
			return items;
		}
	}

	public class SuppliersCacheService : CacheService
	{
		public override string CacheIndexKey
		{
			get { return "suppliers"; }
		}
	}
}