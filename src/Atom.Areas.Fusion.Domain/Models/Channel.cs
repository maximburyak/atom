namespace Atom.Areas.Fusion.Domain.Models
{
	public class Channel : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string Code { get; set; }
		public virtual string Description { get; set; }
		public virtual short OrderBy { get; set; }
	}
}
