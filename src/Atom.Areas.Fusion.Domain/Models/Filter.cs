namespace Atom.Areas.Fusion.Domain.Models
{
	public class Filter : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }
		public virtual string FilterValue { get; set; }
		public virtual bool IsDefault { get; set; }

		public virtual string DisplayText()
		{
			return Description + (IsDefault ? " [DEFAULT]" : "");
		}
	}
}
