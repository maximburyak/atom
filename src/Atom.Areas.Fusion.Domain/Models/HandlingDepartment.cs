using System.ComponentModel;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class HandlingDepartment : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string Email { get; set; }
		public virtual string Description { get; set; }
	}

	// These are not used in the Domain logic
	// but are nice to have a textual representation
	public enum HandlingDepartmentTypeEnum
	{
		[Description("None")]
		None = -1,
		[Description("IT")]
		IT = 0,
		[Description("Infrastructure")]
		Infrastructure = 1,
		[Description("Web")]
		Web = 2,
		[Description("BathInfrastructure")]
		BathInfrastructure = 3,
		[Description("Validator2")]
		Validator2 = 20,
		[Description("PMO")]
		Pmo = 30, // renamed from Change board
		[Description("Guild")]
		Guild = 41,
        [Description("Validator2laptops")]
		V2LaptopSupport = 42,
        [Description("HFS")]
        Hfs = 43
	}
}

