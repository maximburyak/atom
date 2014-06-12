using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public enum WorkItemClosureReason
	{
		[Description("Not Completed with Satisfactory Detail")]
		NoDetail = 10,
		[Description("No Business Benefit")]
		NoBusinessBenefit = 20,
		[Description("Not Required")]
		NotRequired = 30,
		[Description("Work Completed")]
		WorkCompleted = 40,
		[Description("Upgraded to Project")]
		UpgradedToProject = 50,
	}
}
