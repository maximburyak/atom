using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public enum SupportIncidentStatus
	{
		[Description("Open")]
		Open = 0,
		[Description("In Progress")]
		InProgress = 10,
		[Description("On Hold")]
		OnHold = 20,
		[Description("Closed")]
		Closed = 30
	}
}
