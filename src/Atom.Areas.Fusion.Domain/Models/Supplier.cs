namespace Atom.Areas.Fusion.Domain.Models
{
	public class Supplier
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual bool DirectFulfil { get; set; }
		public virtual short OrderBy { get; set; }
	}
}
