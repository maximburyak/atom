using System.Collections.Generic;
using Atom.Areas.Suppliers.Domain.Models;
using Atom.Main.Services.Paging;

namespace Atom.Main.Areas.Suppliers.Models
{
	public class ExistingMappingViewModel : CommonViewModel
	{
		public IPagedList<SupplierCategoryMappingCms> PagedList(int currentPageIndex, int defaultPageSize)
		{
			return Items.ToPagedList(currentPageIndex, defaultPageSize);
		}

		public IPagedList<SupplierCategoryMappingCms> PagedList()
		{
			return Items.ToPagedList(CurrentPageIndex, DefaultPageSize);
		}

		public IList<SupplierCategoryMappingCms> Items { get; set; }

		public bool? Disabled { get; set; }
		public bool? Ignored { get; set; }
		public IList<SupplierCms> Suppliers { get; set; }
	}
}