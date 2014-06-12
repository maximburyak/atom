using System;

namespace Atom.Areas.Fusion.Domain.Models
{
	public interface IAuditableFusionEntity
	{
		DateTime? CreateDate { get; set; }
		DateTime? AlteredDate { get; set; }
		User CreatedBy { get; set; }
		User AlteredBy { get; set; }
	}

	public abstract class AuditableFusionEntity : IAuditableFusionEntity
	{
		public virtual DateTime? CreateDate { get; set; }
		public virtual DateTime? AlteredDate { get; set; }
		public virtual User CreatedBy { get; set; }
		public virtual User AlteredBy { get; set; }
	}
	
}
