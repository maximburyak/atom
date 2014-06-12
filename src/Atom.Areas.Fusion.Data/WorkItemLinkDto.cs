using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Areas.Fusion.Data
{
	public class WorkItemLinkDto
	{
		public int Id { get; set; }
		public int FromWorkItemId { get; set; }
		public int RelatesToWorkItemId { get; set; }
		public WorkItemTypeEnum RelatesToType { get; set; } 
	}
}