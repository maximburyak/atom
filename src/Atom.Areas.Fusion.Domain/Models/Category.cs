using System.Collections.Generic;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class Category : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }
		public virtual bool Enabled { get; set; }
		public virtual IList<AdditionalInfoType> AdditionalInfo { get; set; }
	}
}