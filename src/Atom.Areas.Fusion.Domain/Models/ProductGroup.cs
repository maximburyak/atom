namespace Atom.Areas.Fusion.Domain.Models
{
	public class ProductGroup : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual int v2CatId { get; set; }
		public virtual short OrderBy { get; set; }			
	}
}
