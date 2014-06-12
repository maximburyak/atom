
using System.Collections.Generic;
using Atom.Main.Services.Paging;

namespace Atom.Main.Areas.Suppliers.Models
{
	public class MissingV2MappingViewModel : MissingV2CommonViewModel
	{
		public IPagedList<SupplierFeedsLoadItemMapping> PagedList(int currentPageIndex, int defaultPageSize)
		{
			return Items.ToPagedList(currentPageIndex, defaultPageSize);
		}
		public IPagedList<SupplierFeedsLoadItemMapping> PagedList()
		{
			return Items.ToPagedList(CurrentPageIndex, DefaultPageSize);
		}

		public IEnumerable<SupplierFeedsLoadItemMapping> Items { get; set; }

	}
}