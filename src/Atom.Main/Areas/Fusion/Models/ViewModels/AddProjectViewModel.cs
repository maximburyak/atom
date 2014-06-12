using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Main.Areas.Fusion.Models.ViewModels
{
	public class AddProjectViewModel : BaseWorkItemViewModel
	{
		public AddProjectViewModel(WorkItem workItem)
		{
			WorkItem = workItem;
		}
		public Project Project
		{
			get
			{
				return (Project)WorkItem;
			}
		}
	}
}
