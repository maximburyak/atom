using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public enum SeverityEnum
	{
		[Description("Low")]
		Low = 1,
		[Description("Medium")]
		Medium = 2, 
		[Description("High")]
		High = 3
	}
}
