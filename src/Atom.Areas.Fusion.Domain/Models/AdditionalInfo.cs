using System.Collections.Generic;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class AdditionalInfoType : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }
		public virtual IList<Category> Categories { get; set; }
	}

	public class AdditionalInfo : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual AdditionalInfoType InfoType { get; set; }
		public virtual string Value { get; set; }
	}
}
