namespace Atom.Areas.Fusion.Domain.Models
{
	public class WorkItemSupplier : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual Supplier Supplier { get; set; }
	}
}
