using System.Collections.Generic;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class Area : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }
		public virtual IList<Category> Categories { get; set; }
		public virtual HandlingDepartment HandlingDepartment { get; set; }
		public virtual bool Enabled { get; set; }
		public virtual int Sequence { get; set; }
	}
}