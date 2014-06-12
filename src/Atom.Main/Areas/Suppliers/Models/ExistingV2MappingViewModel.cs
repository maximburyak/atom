using System.Collections.Generic;
using System.Linq;
using Atom.Areas.Suppliers.Domain.Models;
using Atom.Main.Services.Paging;

namespace Atom.Main.Areas.Suppliers.Models
{
	public class ExistingV2MappingViewModel : CommonViewModel
	{
		public bool? Disabled { get; set; }
		public IEnumerable<SupplierCategoryMappingV2> Items { get; set; }

		public IEnumerable<SupplierCms> Suppliers { get; set; }
		public IEnumerable<SupplierCms> PpSuppliers
		{
			get { return Suppliers.Where(x => x.NonPpSupplier == false).OrderBy(x => x.Name); }
		}

		public IEnumerable<SupplierCms> NonPpSuppliers
		{
			get { return Suppliers.Where(x => x.NonPpSupplier).OrderBy(x => x.Name); }
		}

		public IPagedList<SupplierCategoryMappingV2> PagedList(int currentPageIndex, int defaultPageSize)
		{
			return Items.ToPagedList(currentPageIndex, defaultPageSize);
		}
		public IPagedList<SupplierCategoryMappingV2> PagedList()
		{
			return Items.ToPagedList(CurrentPageIndex, DefaultPageSize);
		}
	}
}