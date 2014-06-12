using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public enum WorkItemTypeEnum
	{
		[Description("Incident")]
		Incident = 1,
		[Description("Crf")]
		Crf = 2,
		[Description("Project")]
		Project = 3
	}
}
