using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public enum WorkItemStatus
	{
		[Description("Awaiting Approval")]
		AwaitingApproval = 0,
		[Description("Open")]
		Open = 5,
		[Description("In Progress")]
		InProgress = 10,
		[Description("On Hold")]
		OnHold = 20,
		[Description("Closed")]
		Closed = 30
	}
}
