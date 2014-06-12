namespace Atom.Areas.Fusion.Domain.Models
{
	public class InsuranceCompany 
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string DisplayName { get; set; }
		public virtual short OrderBy { get; set; }
	}
}
