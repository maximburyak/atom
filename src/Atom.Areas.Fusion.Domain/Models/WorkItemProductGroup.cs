namespace Atom.Areas.Fusion.Domain.Models
{
	public class WorkItemProductGroup : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual ProductGroup ProductGroup { get; set; }
	}
}
