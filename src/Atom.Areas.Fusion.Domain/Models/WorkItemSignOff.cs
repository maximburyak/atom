using System;
namespace Atom.Areas.Fusion.Domain.Models
{
	public class WorkItemSignOff : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual WorkItem Workitem { get; set; }
		public virtual SignOffTypeEnum SignOffType { get; set; }
		public virtual DateTime? SignedOff { get; set; }
		public virtual User SignedOffBy { get; set; }
	}
}
