using System.Collections.Generic;
using Atom.Areas.Suppliers.Domain.Models;
using Atom.Main.Services.Paging;

namespace Atom.Main.Areas.Suppliers.Models
{
	public class MissingMappingViewModel : CommonViewModel
	{
		public IPagedList<SupplierFeedsLoadItemMapping> PagedList(int currentPageIndex, int defaultPageSize)
		{
			return Items.ToPagedList(currentPageIndex, defaultPageSize);
		}
		public IPagedList<SupplierFeedsLoadItemMapping> PagedList()
		{
			return Items.ToPagedList(CurrentPageIndex, DefaultPageSize);
		}
		public IList<SupplierFeedsLoadItemMapping> Items { get; set; }
		public IList<SupplierCms> Suppliers { get; set; }
		public IList<Format> Formats { get; set; }
	}
}