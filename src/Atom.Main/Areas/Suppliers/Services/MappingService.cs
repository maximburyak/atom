using System;
using System.Collections.Generic;
using System.Linq;
using Atom.Areas.Suppliers.Data.Query;
using Atom.Areas.Suppliers.Domain.Models;
using Atom.Main.Areas.Suppliers.Models;
using BeValued.Data.NHibernate;
using BeValued.Utilities.Extensions;
using NHibernate;

namespace Atom.Main.Areas.Suppliers.Services
{
	public class MappingService
	{
		private readonly ISession _session;
		private readonly SuppliersCacheManager _cacheManager;
		private readonly INHibernateRepository<SupplierCategoryMappingCms> _supplierMappingRepo;
		public MappingService(ISession session)
		{
			_session = session;
			_cacheManager = new SuppliersCacheManager(session);
			_supplierMappingRepo = new NHibernateRepository<SupplierCategoryMappingCms>(session);
		}
		public IList<SupplierCms> Suppliers()
		{
			return _cacheManager.Suppliers();
		}

		public IList<SupplierFeedsLoadItemMapping> MissingMappings(int supplierId, string Level1, string Level2)
		{
			if (supplierId == 0)
				return new List<SupplierFeedsLoadItemMapping>();

			var fromCache = _cacheManager.MissingMappings(supplierId);
			var items = fromCache;

			if (!string.IsNullOrEmpty(Level1))
				items = items.Where(x => x.Level1.Contains(Level1, StringComparison.InvariantCultureIgnoreCase)).ToList();

			if (!string.IsNullOrEmpty(Level2))
				items = items.Where(x => x.Level2.Contains(Level2, StringComparison.InvariantCultureIgnoreCase)).ToList();
			return items;
		}

		public MissingMappingViewModel MissingModel(int supplierId, string Level1, string Level2, int currentPageIndex, int defaultPageSize)
		{
			return new MissingMappingViewModel
					   {
						   Suppliers = Suppliers(),
						   SupplierId = supplierId,
						   Level1S = Level1,
						   Level2S = Level2,
						   CurrentPageIndex = currentPageIndex,
						   DefaultPageSize = defaultPageSize,
						   Items = MissingMappings(supplierId, Level1, Level2),
						   Formats = Formats()
					   };
		}

		public ExistingMappingViewModel ExistingModel(int supplierId, bool? Disabled, bool? Ignored, string Level1S, string Level2S, int currentPageIndex, int defaultPageSize)
		{
			return new ExistingMappingViewModel
					   {
						   Suppliers = Suppliers(),
						   SupplierId = supplierId,
						   Disabled = Disabled,
						   Ignored = Ignored,
						   Level1S = Level1S,
						   Level2S = Level2S,
						   CurrentPageIndex = currentPageIndex,
						   DefaultPageSize = defaultPageSize,
						   Items = ExistingMappings(supplierId, Disabled, Ignored, Level1S, Level2S)
					   };
		}

		public IList<SupplierCategoryMappingCms> ExistingMappings(int supplierId, bool? Disabled, bool? Ignored, string Level1, string Level2)
		{
			if (supplierId == 0)
				return new List<SupplierCategoryMappingCms>();

			var fromCache = _cacheManager.ExistingMappings(supplierId);
			var items = fromCache;

			if (Disabled.HasValue)
				items = items.Where(x => x.Disabled == Disabled.Value).ToList();

			if (Ignored.HasValue)
				items = items.Where(x => x.Ignored == Ignored.Value).ToList();

			if (!string.IsNullOrEmpty(Level1))
				items = items.Where(x => x.Level1.Contains(Level1, StringComparison.InvariantCultureIgnoreCase)).ToList();

			if (!string.IsNullOrEmpty(Level2))
				items = items.Where(x => x.Level2.Contains(Level2, StringComparison.InvariantCultureIgnoreCase)).ToList();
			return items;
		}

		private IList<Format> Formats()
		{
			return _cacheManager.Formats();
		}

		public void NewEntry(SupplierCategoryMappingCms mapping, string user)
		{
			mapping.SetDefaults(user);
			_cacheManager.RemoveCacheId("missingmappings-" + mapping.SupplierID);
			_cacheManager.RemoveCacheId("existingmappings-" + mapping.SupplierID);
			_supplierMappingRepo.Save(mapping);
		}

		public void AmendDisabled(SupplierCategoryMappingCms m, string userName)
		{
			var newMapping = new ListSupplierCategoryMappingCms { SupplierId = m.SupplierID, Level1 = m.Level1, Level2 = m.Level2, Level3 = m.Level3 ?? "", Level4 = m.Level4 ?? "" }.GetQuery(_session).Future
				   <SupplierCategoryMappingCms>().First();
			newMapping.Disabled = m.Disabled;
			_cacheManager.RemoveCacheId("missingmappings-" + m.SupplierID);
			_cacheManager.RemoveCacheId("existingmappings-" + m.SupplierID);
			_supplierMappingRepo.Save(newMapping);
		}

		public void AmendIgnored(SupplierCategoryMappingCms m, string userName)
		{
			var newMapping = new ListSupplierCategoryMappingCms { SupplierId = m.SupplierID, Level1 = m.Level1, Level2 = m.Level2, Level3 = m.Level3 ?? "", Level4 = m.Level4 ?? "" }.GetQuery(_session).Future
					<SupplierCategoryMappingCms>().First();
			if (m.Ignored)
			{
				newMapping.Ignored = m.Ignored;
				_supplierMappingRepo.Save(newMapping);
			}
			else
			{ // otherwise we want to stop ignoring, and allow the mapping.
				_supplierMappingRepo.Delete(newMapping);
			}

			_cacheManager.RemoveCacheId("missingmappings-" + m.SupplierID);
			_cacheManager.RemoveCacheId("existingmappings-" + m.SupplierID);

		}
	}
}
