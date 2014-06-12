namespace Atom.Areas.Fusion.Domain.Models
{
	public class WorkItemLink : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual WorkItem Item { get; set; }
		public virtual WorkItem RelatesTo { get; set; }
	}
}