using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class CrfDetailsBaseViewModel : BaseWorkItemViewModel
	{
		public CrfDetailsBaseViewModel(WorkItem workItem)
		{
			WorkItem = workItem;
		}
		public Crf Crf
		{
			get { return (Crf)WorkItem; }
		}

		public bool CanStartWorkOnCrf()
		{
			return (Crf.EstimatedStartDate.HasValue && Crf.EstimatedUnitsOfWork.HasValue);
		}

		public bool CommentAdded;
	}
}
