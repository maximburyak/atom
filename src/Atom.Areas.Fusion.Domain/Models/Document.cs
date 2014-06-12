namespace Atom.Areas.Fusion.Domain.Models
{
	public class Document : AuditableFusionEntity
	{
		public virtual int Id { get; set; }
		public virtual decimal Revision { get; set; }
		public virtual string FileName { get; set; }
		public virtual byte[] Data { get; set; }
	}
}
