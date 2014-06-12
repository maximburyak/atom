using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class ProjectDetailsBaseViewModel : BaseWorkItemViewModel
	{
		public ProjectDetailsBaseViewModel(WorkItem workItem)
		{
			WorkItem = workItem;
		}
		public Project Project
		{
			get { return (Project)WorkItem; }
		}
		public bool CommentAdded;
	}
}
