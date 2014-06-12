using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public enum RequestTypeEnum
	{

		Bug = 1,
		Error = 2,
		[Description("Training Required")]
		TrainingRequired = 3,
		Faulty = 4,
		Request = 5,
		[Description("Claim Fix")]
		ClaimFix = 6
	}
}
