namespace Atom.Areas.Fusion.Domain.Models
{
	public class WorkItemChannel : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual Channel Channel { get; set; }
	}
}
