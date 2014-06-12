using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class ProjectDetailsViewModel : BaseWorkItemViewModel
	{
		public ProjectDetailsViewModel(WorkItem workItem)
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
