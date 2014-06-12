using System.Collections.Generic;
using System.Web;
using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class SearchViewModel
	{
		public SearchViewModel() { }

		public SearchViewModel(string searchToPerfom, string actionToPerform)
		{
			search = searchToPerfom;
			action = actionToPerform;
			filters = new List<Filter>();
		}
		public string search;
		public string action;
		public IList<Filter> filters;
		public User User;
		public Filter DefaultFilter;
	    public int workItemIdToLinkTo;
	    public WorkItemTypeEnum WorkItemType;

		public bool InHomeScreen()
		{
			return SiteMap.CurrentNode == null ? true : SiteMap.CurrentNode.Title != "My Work";
		}
	}
}