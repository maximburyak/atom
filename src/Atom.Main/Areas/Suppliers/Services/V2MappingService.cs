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
	public class V2MappingService
	{
		private readonly ISession _session;
		private readonly INHibernateRepository<SupplierCategoryMappingV2> _v2MappingRepo;
		private readonly INHibernateRepository<SupplierCategoryMappingV2Powerplay> _v2PpMappingRepo;

		public V2MappingService(ISession session,
			INHibernateRepository<SupplierCategoryMappingV2> v2MappingRepo,
			INHibernateRepository<SupplierCategoryMappingV2Powerplay> v2PpMappingRepo
		)
		{
			_session = session;
			_v2MappingRepo = v2MappingRepo;
			_v2PpMappingRepo = v2PpMappingRepo;
		}

		public MissingV2MappingViewModel MissingV2Model(int supplierId, string Level1, string Level2, int currentPageIndex, int defaultPageSize, bool includeCategories = false)
		{
			var model = new MissingV2MappingViewModel
							{
								Suppliers = ListSuppliers(),
								Items = MissingMappings(supplierId, Level1, Level2),
								SupplierId = supplierId,
								Level1S = Level1,
								Level2S = Level2,
								CurrentPageIndex = currentPageIndex,
								DefaultPageSize = defaultPageSize,
							};
			if (includeCategories)
				model.Categories = ListCategories();
			return model;
		}

		private IEnumerable<SupplierFeedsLoadItemMapping> MissingMappings(int supplierId, string level1, string level2)
		{
			if (supplierId == 0)
				return new List<SupplierFeedsLoadItemMapping>();

			var queriedItems = new ListSupplierFeedLoadItemsWithNoV2Mappings { SupplierId = supplierId }.GetQuery(_session).Future<SupplierFeedsLoadItem>();
			var items = (from i in queriedItems
						 select new SupplierFeedsLoadItemMapping { SupplierId = i.SupplierId, Level1 = i.Level1, Level2 = i.Level2, Level3 = i.Level3, Level4 = i.Level4 }).ToList();

			if (!string.IsNullOrWhiteSpace(level1))
				items = items.Where(x => x.Level1.Contains(level1, StringComparison.InvariantCultureIgnoreCase)).ToList();

			if (!string.IsNullOrWhiteSpace(level2))
				items = items.Where(x => x.Level2.Contains(level2, StringComparison.InvariantCultureIgnoreCase)).ToList();
			return items;
		}

		private IEnumerable<Validator2Category> ListCategories()
		{
			var categories = _session.QueryOver<Validator2Category>().Future();
			return categories;
		}

		private IEnumerable<SupplierCms> ListSuppliers()
		{
			var suppliers = _session.QueryOver<SupplierCms>().Future();
			return suppliers;
		}

		public void NewSupplierCategoryMappingV2(SupplierCategoryMappingV2 mapping, string user)
		{
			mapping.SetDefaults(user);
			_v2MappingRepo.Save(mapping);
		}

		public ExistingV2MappingViewModel ExistingV2MappingModel(int supplierId, bool? disabled, string level1S, string level2S, int currentPageIndex, int defaultPageSize)
		{
			return new ExistingV2MappingViewModel()
					{
						CurrentPageIndex = currentPageIndex,
						DefaultPageSize = defaultPageSize,
						Disabled = disabled,
						Level1S = level1S,
						Level2S = level2S,
						Suppliers = ListSuppliers(),
						SupplierId = supplierId,
						Items = ExistingV2MappingList(supplierId, disabled, level1S, level2S)
					};
		}

		private IEnumerable<SupplierCategoryMappingV2> ExistingV2MappingList(int supplierId, bool? disabled, string level1, string level2)
		{
			if (supplierId == 0)
				return new List<SupplierCategoryMappingV2>();

			var queriedItems = new ListSupplierCategoryMappingV2 { SupplierId = supplierId, Level1 = level2, Level2 = level2 }.GetQuery(_session).Future<SupplierCategoryMappingV2>();

			if (disabled.HasValue)
				queriedItems = queriedItems.Where(x => x.Disabled == disabled.Value).ToList();

			return queriedItems;
		}

		public void AmendV2MappingDisabled(SupplierCategoryMappingV2 m, string userName)
		{
			var dbMapping = new ListSupplierCategoryMappingV2 { SupplierId = m.SupplierID, Level1 = m.Level1, Level2 = m.Level2, Level3 = m.Level3 ?? "", Category = m.Category }.
					GetQuery(_session).UniqueResult<SupplierCategoryMappingV2>();
			dbMapping.Disabled = m.Disabled;
			_v2MappingRepo.Save(dbMapping);
		}

		public MissingV2MappingPpViewModel MissingV2PpModel(string format, string formattype, string category, int currentPageIndex, int defaultPageSize, bool includeCategories = false)
		{
			var model = new MissingV2MappingPpViewModel
			{
				Suppliers = ListSuppliers(),
				Items = MissingPpMappings(category, format, formattype),
				FormatS = format,
				FormatTypeS = formattype,
				Formats = ListFormats(),

				CurrentPageIndex = currentPageIndex,
				DefaultPageSize = defaultPageSize,
			};
			if (includeCategories)
				model.Categories = ListCategories();
			return model;
		}

		private IEnumerable<Format> ListFormats()
		{
			var items = from f in new ListFormatCms().GetQuery(_session).Future<Format>()
						where f != null && !string.IsNullOrWhiteSpace(f.Description)
						select f;
			return items;


		}

		private IEnumerable<V2PpMappingLoadItemViewModelItem> MissingPpMappings(string category, string format, string formattype)
		{
			var queriedItems = new ListSupplierFeedLoadItemsWithNoV2PpMappings { Category = category }.GetQuery(_session).Future<PowerplaySuppliedFeedItem>();
			var items = (from i in queriedItems
						 select new V2PpMappingLoadItemViewModelItem { Format = i.Format.FormatCode, FormatDescription = i.Format.FormatDescription, FormatType = i.FormatType.FormatTypeCode, FormatTypeDescription = i.FormatType.FormatTypeDescription, Class = i.Class.Class, ClassDescription = i.Class.ClassDescription }).ToList();

			if (!string.IsNullOrWhiteSpace(format))
				items = items.Where(x => x.Format.Contains(format, StringComparison.InvariantCultureIgnoreCase)).ToList();

			if (!string.IsNullOrWhiteSpace(formattype))
				items = items.Where(x => x.FormatType.Contains(formattype, StringComparison.InvariantCultureIgnoreCase)).ToList();

			return items;
		}

		public void NewSupplierCategoryMappingV2PP(SupplierCategoryMappingV2Powerplay mapping, string userName)
		{
			mapping.SetDefaults(userName);
			_v2PpMappingRepo.Save(mapping);
		}
	}
}