namespace Atom.Areas.Fusion.Domain.Models
{
	public class ClosureReason : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }
        public virtual string Department { get; set; }
        public virtual bool Enabled { get; set; }
	}
}