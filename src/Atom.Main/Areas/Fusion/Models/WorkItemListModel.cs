using System.Collections.Generic;

namespace Atom.Main.Areas.Fusion.Models
{
	public class WorkItemListModel
	{
		public string SearchDisplayText;
		public string SearchFilter;
		public List<WorkItemListItemModel> Items;
	}
}