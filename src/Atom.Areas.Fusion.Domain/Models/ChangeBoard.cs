using System;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class ChangeBoard : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual DateTime NextMeeting { get; set; }
	}
}
