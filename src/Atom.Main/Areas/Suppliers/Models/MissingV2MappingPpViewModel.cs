using System.Collections.Generic;
using Atom.Areas.Suppliers.Domain.Models;
using Atom.Main.Services.Paging;

namespace Atom.Main.Areas.Suppliers.Models
{
	public class MissingV2MappingPpViewModel : MissingV2CommonViewModel
	{
		public string FormatS;
		public string FormatTypeS;

		public IEnumerable<Format> Formats { get; set; }

		public IPagedList<V2PpMappingLoadItemViewModelItem> PagedList(int currentPageIndex, int defaultPageSize)
		{
			return Items.ToPagedList(currentPageIndex, defaultPageSize);
		}
		public IPagedList<V2PpMappingLoadItemViewModelItem> PagedList()
		{
			return Items.ToPagedList(CurrentPageIndex, DefaultPageSize);
		}

		public IEnumerable<V2PpMappingLoadItemViewModelItem> Items { get; set; }

	}
}