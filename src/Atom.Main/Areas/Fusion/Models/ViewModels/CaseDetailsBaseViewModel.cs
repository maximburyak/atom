using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class CaseDetailsBaseViewModel : BaseWorkItemViewModel
	{
		public CaseDetailsBaseViewModel(WorkItem workItem)
		{
			WorkItem = workItem;
		}
		public SupportIncident Incident
		{
			get
			{
				return (SupportIncident)(WorkItem);
			}
		}
		public IEnumerable<ClosureReason> ClosureReasons { get; set; }
        public bool CommentAdded;
	}
}
