using System.Collections.Generic;
using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Areas.Fusion.Domain
{
	public class SupportProfile : AuditableFusionEntity
	{
		public virtual int UserId { get; set; }
		public virtual IList<Avatar> Avatars { get; set; }
		public virtual IList<Signature> Signatures { get; set; }
		public virtual IList<Filter> Filters { get; set; }
		public virtual int CurrentAvatar { get; set; }
		public virtual int CurrentSignature { get; set; }
		public virtual bool ShowFilters { get; set; }
		public virtual bool RefreshSearch { get; set; }
		public virtual bool IsAssignedToAuto { get; set; }

		public SupportProfile()
		{
			Avatars = new List<Avatar>();
			Signatures = new List<Signature>();
			Filters = new List<Filter>();
		}
	}
}
