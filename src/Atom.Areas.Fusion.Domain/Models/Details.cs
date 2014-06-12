namespace Atom.Areas.Fusion.Domain.Models
{
	public class Details : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual Area Area { get; set; }
		public virtual Category Category { get; set; }
		public virtual RequestTypeEnum FaultType { get; set; }
	}
}