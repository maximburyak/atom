namespace Atom.Areas.Fusion.Domain.Models
{
	public class Subscription : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual User User { get; set; }
		public virtual WorkItem WorkItem { get; set; }	
	}
}
