namespace Atom.Areas.Fusion.Domain.Models
{
	public class WorkItemInsuranceCompany : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual InsuranceCompany InsuranceCompany { get; set; }
	}
}
