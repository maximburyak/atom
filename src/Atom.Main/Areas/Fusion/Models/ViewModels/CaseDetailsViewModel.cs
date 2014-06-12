using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class CaseDetailsViewModel : CaseDetailsBaseViewModel
	{
		public CaseDetailsViewModel(WorkItem workItem) : base(workItem)
		{
			WorkItem = workItem;
		}		
	}
}