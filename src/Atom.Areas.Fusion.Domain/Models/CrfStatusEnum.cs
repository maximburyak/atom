using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public enum CrfStatus
	{
		// Awaiting 'Change Board'
		[Description("Requested")]
		Requested = 0,
		// Approved, not assigned
		[Description("Approved")]
		Approved = 5,
		// Assigned
		[Description("In Progress")]
		InProgress = 10,
		[Description("Rejected")]
		Rejected = 20,
		[Description("On Hold")]
		OnHold = 30,
		[Description("Completed")]
		Completed = 40
	}
}
