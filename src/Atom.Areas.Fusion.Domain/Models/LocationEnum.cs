using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public enum LocationEnum
	{
		[Description("Eastbourne - Unit 1")]
		Unit1 = 1,
		[Description("Eastbourne")]
		Unit2 = 2,
		[Description("Home Address")]
		Home = 3,
		[Description("Edinburgh")]
		Edinburgh = 50,
		[Description("Halifax")]
		Halifax = 60,
        [Description("Orpington")]
        Orpington = 90,
		[Description("Warrington")]
		Warrington = 100,
		[Description("Bath")]
		Bath = 200,
        [Description("Albion-London")]
        Albion = 210,
        [Description("Australia")]
        Australia = 300
	}
}
