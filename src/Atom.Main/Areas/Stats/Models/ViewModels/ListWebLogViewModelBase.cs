using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atom.Main.Areas.Stats.Models.ViewModels
{
	public class ListWebLogViewModelBase
	{
		public string SortColumn { get; set; }

		public bool IsSortDesc { get; set; }
		
		public IEnumerable<WebsiteName> AvailableSites { get; set; }

		public string SelectedWebsite { get; set; }

		public string PathFilter { get; set; }
	}
}